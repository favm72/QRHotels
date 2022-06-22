using Common;
using DataLayer;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DataOracleLayer;
using System.Net;
using System.Collections.Generic;

namespace LogicLayer
{
	public class IntakeBL : BaseBL
	{
		private MyContext context { get; set; }
		private OracleContext oracleContext { get; set; }
		private MyConfig myConfig { get; set; }
		public IntakeBL(MyContext context, OracleContext oracleContext, MyConfig myConfig)
		{
			this.context = context;
			this.oracleContext = oracleContext;
			this.myConfig = myConfig;
		}
		public async Task<ResponseBE> List(DummyBE dummyBE)
		{
			return await GetResponse(dummyBE, MyRole.Client, async (response) =>
			{
				var userData = await (from r in context.Reservation
								  join h in context.Hotels on r.HotelCode equals h.HotelCode
								  where r.Active && r.Id == dummyBE.TokenBE.Id
								  select new
								  {
									  hotel = h.Rmc,
									  reseCode = r.ReseCode,
									  year = r.ReseYear
								  }).FirstOrDefaultAsync();

				var list = await oracleContext.IntakeList(userData.hotel, userData.reseCode.ToString(), userData.year.ToString());
				//var list = new List<Intake>();
				//list.Add(new Intake()
				//{
				//	ReseCodi = "2021123456",
				//	TipoCta = "A",
				//	Seccion = "Seccion",
				//	Servicio = "Servicio",
				//	Moneda = "SOL",
				//	Importe = 10,
				//	ImporteSol = 10,
				//	Ticket = "lavanderia",
				//	PtoVta = "V",
				//});

				var data = (from x in list
							group x by new
							{
								FechaConsumo = x.FechaConsumo,
								Seccion = x.Seccion,
								Servicio = x.Servicio,
								Ticket = x.Ticket,
								Currency = x.Currency
							} into g
							select new IntakeBE()
							{
								IntakeDate = g.Key.FechaConsumo,
								Section = g.Key.Seccion,
								Service = g.Key.Servicio,
								Ticket = g.Key.Ticket,
								Currency = g.Key.Currency,
								Amount = g.Sum(y => y.Amount)
							})
							.OrderBy(z => z.IntakeDate)
							.ThenBy(z => z.Ticket)
							.ThenBy(z => z.Currency)
							.ToList();

				var group = new List<GroupIntakeBE>();
				var currentGroup = new GroupIntakeBE();
				Func<int, bool> samegroup = (x) => data[x].TicketNo == data[x - 1].TicketNo && data[x].Currency == data[x - 1].Currency;

				for (int i = 0; i < data.Count; i++)
				{
					if (i == 0 || !samegroup(i))
					{
						currentGroup = new GroupIntakeBE();
						currentGroup.Ticket = data[i].Ticket;
						currentGroup.Currency = data[i].Currency;
						currentGroup.Items = new List<IntakeBE>() { data[i] };
						currentGroup.Total = data[i].Amount;
						group.Add(currentGroup);
					}
					else
					{
						currentGroup.Items.Add(data[i]);
						currentGroup.Total += data[i].Amount ?? 0;
					}
				}
				//var group = (from x in data
				//			 orderby x.IntakeDate ascending
				//			 group x by new
				//			 {
				//				 Ticket = x.Ticket,
				//				 TicketNo = x.TicketNo,
				//				 Year = x.Year,
				//				 Cashier = x.Cashier,
				//				 Currency = x.Currency
				//			 } into g
				//			 select new
				//			 {
				//				 Ticket = g.Key.Ticket,
				//				 TicketNo = g.Key.TicketNo,
				//				 Year = g.Key.Year,
				//				 Cashier = g.Key.Cashier,
				//				 Currency = g.Key.Currency,
				//				 Total = g.Sum(y => y.Amount),
				//				 Items = g.ToList()
				//			 }).ToList();

				response.data = group;
			});
		}
		public async Task<ResponseBE> Detail(IntakeDetailBE intakeDetailBE)
		{
			return await GetResponse(intakeDetailBE, MyRole.Client, async (response) =>
			{
				var userData = await (from r in context.Reservation
									  join h in context.Hotels on r.HotelCode equals h.HotelCode
									  where r.Active && r.Id == intakeDetailBE.TokenBE.Id
									  select new { hotel = h.Rmc, hotelCode = h.HotelCode }).FirstOrDefaultAsync();

				var ticket = new IntakeBE();
				ticket.Ticket = intakeDetailBE.Ticket;
				var list = await oracleContext.IntakeDetail(userData.hotel, ticket.TicketNo, ticket.Cashier, ticket.Year);
				var data = (from x in list
							orderby x.NroTicket descending, x.Producto
							select new
							{
								Product = x.Producto,
								Amount = x.Venta,
								Discount = x.Dscto,
								Surcharge = x.Recargo
							}).ToList();
				response.data = new { detail = data, hotelCode = userData.hotelCode };
			});
		}
		public async Task<ResponseBE> Reasons(DummyBE dummyBE)
		{
			return await GetResponse(dummyBE, MyRole.Client, async (response) =>
			{
				var data = await (from x in context.IntakeReason
								  where x.Active
								  select new
								  {
									  Id = x.Id,
									  Description = x.Description
								  }).ToListAsync();

				response.data = data;
			});
		}
		public async Task<ResponseBE> CanAgree(DummyBE dummyBE)
		{
			return await GetResponse(dummyBE, MyRole.Client, async (response) =>
			{
				var userData = await (from r in context.Reservation
									  join h in context.Hotels on r.HotelCode equals h.HotelCode
									  where r.Active && r.Id == dummyBE.TokenBE.Id
									  select new
									  {
										  Hours = h.HoursToAgreeBeforeCheckout,
										  Checkout = r.CheckOut
									  }).FirstOrDefaultAsync();

				if (userData == null || userData.Checkout == null)
				{
					response.data = false;
					return;
				}

				var hours = (userData.Checkout.Value - DateTime.Now).TotalHours;
				response.data = hours > 0 && hours < userData.Hours;
			});
		}
		public async Task<ResponseBE> SendEmail(IntakeMailBE intakeMailBE)
		{
			return await GetResponse(intakeMailBE, MyRole.Client, async (response) =>
			{
				var userData = await (from r in context.Reservation
									  join h in context.Hotels on r.HotelCode equals h.HotelCode
									  where r.Active && r.Id == intakeMailBE.TokenBE.Id
									  select new { reservation = r, hotel = h }).FirstOrDefaultAsync();

				var reason = await (from x in context.IntakeReason
									where x.Id == intakeMailBE.ReasonId
									select x.Description).FirstOrDefaultAsync();

				System.Net.Mail.MailMessage correo = new System.Net.Mail.MailMessage();
				correo.From = new System.Net.Mail.MailAddress(myConfig.EmailFrom);
				correo.IsBodyHtml = true;
				correo.Priority = System.Net.Mail.MailPriority.High;
				correo.To.Add(userData.hotel.Email);
				correo.Subject = $"QR Hoteles Reservación {userData.reservation.ReseCode}";
				correo.Body = @$"El cliente:
				<br>Hotel: {userData.hotel.ShortDescription}
				<br>Código Reserva: {userData.reservation.ReseCode}
				<br>Año Reserva: {userData.reservation.ReseYear}
				<br>Habitación: {userData.reservation.RoomCode}
				<br>Nombre: {userData.reservation.FirstName} {userData.reservation.LastName}
				<br>
				<br>Ha presentado inconformidad:
				<br>Motivo: {reason}
				<br>Contenido: {intakeMailBE.Description}
				";

				System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
				smtp.Host = myConfig.EmailHost;
				smtp.Port = int.Parse(myConfig.EmailPort);
				smtp.EnableSsl = true;
				smtp.Credentials = new NetworkCredential(myConfig.EmailFrom, myConfig.EmailPass);

				await smtp.SendMailAsync(correo);

				response.data = true;
			});
		}
	}
}
