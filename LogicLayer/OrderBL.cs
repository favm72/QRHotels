using Common;
using DataLayer;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;

namespace LogicLayer
{
	public class OrderBL : BaseBL
	{
		private MyContext context;
		public OrderBL(MyContext context)
		{
			this.context = context;
		}

		public async Task<ResponseBE> ValidateBreakFast(OrderBE orderBE)
		{
			return await GetResponse(orderBE, MyRole.Client, async (response) =>
			{

				var reservation = await (from r in context.Reservation
										 where r.Active && r.Id == orderBE.TokenBE.Id
										 select r).FirstOrDefaultAsync();

				if (reservation == null)
					throw new TokenException("No se encontró la reservación.");

				var categories = (from c in context.Category
								  join t in context.ProductType on c.Id equals t.IdCategory
								  join p in context.Product on t.Id equals p.IdProductType
								  where p.Active && t.Active
								  && p.HotelCode == reservation.HotelCode
								  && p.IdProvider == 4
								  select new { c, p }).ToList().GroupBy(x => x.c).Select(x => new
								  {
									  Id = x.Key.Id,
									  Name = x.Key.Name,
									  Products = x.Select(y => new
									  {
										  Id = y.p.Id
									  }).ToList()
								  });

				foreach (var c in categories)
				{
					int sum = (from p in c.Products
							   join x in orderBE.Products on p.Id equals x.Id
							   select x.Quantity).Sum();

					if (sum > (reservation.Guests * 5 ?? 0))
					{
						throw new Exception($"No debe exceder la cantidad de {reservation.Guests * 5 ?? 0} productos para la categoría de {c.Name}.");
					}
				}
			});
		}

		public async Task<DateTime> ValidateBreakfastDate(OrderBE orderBE, Func<Task<string>> getHourStart, Func<Task<int?>> getMinutes)
		{
			if (string.IsNullOrWhiteSpace(orderBE.BreakFastDateString))
				throw new MyException("Debe ingresar la fecha para el desayuno.");
			if (orderBE.IdSchedule == 0)
				throw new MyException("Debe seleccionar un horario para el desayuno.");
			if (orderBE.Products == null || orderBE.Products.Count == 0)
				throw new MyException("No se han seleccionado productos para el desayuno.");

			DateTime now = DateTime.Now;
			DateTime breakFastDate;

			bool parsed = DateTime.TryParseExact(orderBE.BreakFastDateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out breakFastDate);
			if (!parsed)
				throw new MyException("Ingrese una fecha para el desayuno válida.");

			if (now.Date > breakFastDate)
				throw new MyException("No es posible agendar un pedido de desayuno para una fecha pasada.");

			string hourStart = await getHourStart();

			if (string.IsNullOrWhiteSpace(hourStart))
				throw new MyException("No se configuró la Hora de inicio correctamente.");

			DateTime bftime;
			parsed = DateTime.TryParseExact($"{orderBE.BreakFastDateString} {hourStart}", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out bftime);
			if (!parsed)
				throw new MyException("Hora de inicio no configurada correctamente.");

			if (now > bftime)
				throw new MyException("Ya pasó el turno seleccionado.");

			int? minutes = await getMinutes();

			if (minutes != null && (bftime - now).TotalMinutes < minutes)
				throw new Exception($"Debe agendar el pedido de desayuno con {minutes} minutos de anticipación.");

			return bftime;
		}
		public async Task<ResponseBE> Save(OrderBE orderBE)
		{
			return await GetResponseTransaction(orderBE, MyRole.Client, context, async (response) =>
			{

				var reservation = await (from r in context.Reservation
										 where r.Active && r.Id == orderBE.TokenBE.Id
										 select r).FirstOrDefaultAsync();

				if (reservation == null)
					throw new TokenException("No se encontró la reservación.");

				OrderHead head = new OrderHead();
				head.Created = DateTime.Now;
				head.Phone = orderBE.Phone;
				head.IdReservation = orderBE.TokenBE.Id;
				head.SubTotal = orderBE.SubTotal;
				head.IdProvider = orderBE.IdProvider;
				head.IdPromo = orderBE.IdPromo;
				head.Total = orderBE.Total;
				if (orderBE.IdPaymType > 0)
					head.IdPaymType = orderBE.IdPaymType;
				if (orderBE.IdIntakeReason > 0)
					head.IdIntakeReason = orderBE.IdIntakeReason;
				head.Viewed = true;
				head.Description = orderBE.Comment;

				if (orderBE.IdProvider == 10)
				{
					var userData = await (from r in context.Reservation
										  join h in context.Hotels on r.HotelCode equals h.HotelCode
										  where r.Active && r.Id == orderBE.TokenBE.Id
										  select new
										  {
											  Hours = h.HoursToAgreeBeforeCheckout,
											  Checkout = r.CheckOut
										  }).FirstOrDefaultAsync();
					if (userData == null)
						throw new MyException("No se encontraron los datos del usuario.");

					if (userData.Checkout == null)
						throw new MyException("La reservación no tiene una fecha de check out definida.");

					if (DateTime.Now > userData.Checkout.Value)
						throw new MyException("La fecha de checkout ya pasó.");

					var hours = (userData.Checkout.Value - DateTime.Now).TotalHours;
					bool canAgree = hours < userData.Hours;

					if (!canAgree)
						throw new MyException($"Sólo se permite la operación {userData.Hours} antes del checkout.");
				}

				if (orderBE.IdProvider == 3)
				{
					if (orderBE.IdLateCheckout == 0)
						throw new MyException("No se seleccionó el servicio de Late Check Out");

					var latecheck = await (from x in context.LateCheckout
										   where x.Id == orderBE.IdLateCheckout
										   && x.HotelCode == reservation.HotelCode
										   && x.Active
										   select x).FirstOrDefaultAsync();
					if (latecheck == null)
						throw new MyException($"No se encontró el servicio de Late Check Out con id {orderBE.IdLateCheckout}");

					if (latecheck.HourLimit != null && DateTime.Now.Hour >= latecheck.HourLimit)
						throw new MyException($"Esta solicitud debe hacerla el mismo día antes de las {latecheck.HourLimit}:00");

					head.IdLateCheckout = orderBE.IdLateCheckout;
				}

				if (orderBE.IdProvider == 4)
				{
					DateTime breakFastDate = await ValidateBreakfastDate(orderBE, async () =>
					{
						return await (from s in context.Schedule
									  where s.Id == orderBE.IdSchedule
									  select s.HourStart).FirstOrDefaultAsync();
					}, async () =>
					{
						return await (from h in context.Hotels
									  where h.HotelCode == reservation.HotelCode
									  select h.MinutesBfast).FirstOrDefaultAsync();
					});

					var productids = orderBE.Products.Select(x => x.Id).ToList();

					var categories = (from c in context.Category
									  join t in context.ProductType on c.Id equals t.IdCategory
									  join p in context.Product on t.Id equals p.IdProductType
									  where p.Active && t.Active
									  && p.IdProvider == 4
									  && p.HotelCode == reservation.HotelCode
									  select new { c, p }).ToList().GroupBy(x => x.c).Select(x => new
									  {
										  Id = x.Key.Id,
										  Name = x.Key.Name,
										  Products = x.Select(y => new
										  {
											  Id = y.p.Id
										  }).ToList()
									  });

					foreach (var c in categories)
					{
						int sum = (from p in c.Products
								   join x in orderBE.Products on p.Id equals x.Id
								   select x.Quantity).Sum();

						if (sum > (reservation.Guests * 5 ?? 0))
						{
							throw new MyException($"No debe exceder la cantidad de {reservation.Guests * 5 ?? 0} productos para la categoría de {c.Name}.");
						}
					}

					head.BreakFastDate = breakFastDate;
					head.IdSchedule = orderBE.IdSchedule;
				}

				OrderDetail detail;
				head.OrderDetail = new List<OrderDetail>();
				if (orderBE.Products != null)
				{
					foreach (var item in orderBE.Products)
					{
						detail = new OrderDetail();
						detail.IdProduct = item.Id;
						detail.Price = item.Price;
						detail.Quantity = item.Quantity;
						detail.Description = item.Comment;
						detail.Modifiers = item.Modifiers;
						head.OrderDetail.Add(detail);
					}
				}

				OrderStatus status = new OrderStatus();
				status.StatusCode = "P";
				//status.Description = "Pendiente";
				status.Active = true;
				status.Created = DateTime.Now;
				head.OrderStatus = new List<OrderStatus>();
				head.OrderStatus.Add(status);

				await context.OrderHead.AddAsync(head);
				await context.SaveChangesAsync();
			});
		}

		public async Task<ResponseBE> Reasons(OrderBE orderBE)
		{
			return await GetResponse(orderBE, MyRole.Both, async (response) =>
			{
				var reasons = await (from r in context.Reason
									 where r.Active
									 select new
									 {
										 Id = r.Id,
										 Description = r.Description
									 }).ToListAsync();

				response.data = reasons;
			});
		}

		public async Task<ResponseBE> NotificationsByClient(OrderBE orderBE)
		{
			return await GetResponse(orderBE, MyRole.Client, async (response) =>
			{
				int IdReservation = orderBE.TokenBE.Id;
				var orders = await (from o in context.OrderHead
									join s in context.OrderStatus on o.Id equals s.IdOrderHead
									join r in context.Reservation on o.IdReservation equals r.Id
									from m in context.Reason.Where(x => x.Id == s.IdReason).DefaultIfEmpty()
									where o.IdReservation == IdReservation
									&& o.Viewed == false
									&& s.Id == context.OrderStatus
										.Where(z => z.IdOrderHead.Value == o.Id)
										.Max(z => z.Id)
									select new
									{
										Id = o.Id,
										StatusCode = s.StatusCode
									}).ToListAsync();

				response.data = orders;
			});
		}

		public async Task<ResponseBE> MarkViewed(OrderBE orderBE)
		{
			return await GetResponse(orderBE, MyRole.Client, async (response) =>
			{
				int IdReservation = orderBE.TokenBE.Id;
				var head = await (from h in context.OrderHead
								  where h.Id == orderBE.Id
								  && h.IdReservation == IdReservation
								  select h).FirstOrDefaultAsync();
				if (head == null)
					throw new MyException($"No se encontró la orden con ID = {orderBE.Id}");

				head.Viewed = true;
				await context.SaveChangesAsync();
			});
		}

		public async Task<ResponseBE> FindByID(OrderBE orderBE)
		{
			return await GetResponse(orderBE, MyRole.Both, async (response) =>
			{
				bool idAdmin = orderBE.TokenBE.IsAdmin;
				int IdReservation = orderBE.TokenBE.Id;
				var order = await (from o in context.OrderHead
								   join s in context.OrderStatus on o.Id equals s.IdOrderHead
								   join p in context.Provider on o.IdProvider equals p.Id
								   join r in context.Reservation on o.IdReservation equals r.Id
								   from m in context.Reason.Where(x => x.Id == s.IdReason).DefaultIfEmpty()
								   from g in context.PaymType.Where(x => x.Id == o.IdPaymType).DefaultIfEmpty()
								   from ir in context.IntakeReason.Where(x => x.Id == o.IdIntakeReason).DefaultIfEmpty()
								   from h in context.Schedule.Where(x => x.Id == o.IdSchedule).DefaultIfEmpty()
								   from l in context.LateCheckout.Where(x => x.Id == o.IdLateCheckout).DefaultIfEmpty()
								   from sn in context.StatusNames.Where(x => x.Code == s.StatusCode).DefaultIfEmpty()
								   where o.Id == orderBE.Id
								   && (o.IdReservation == IdReservation || idAdmin)
								   && s.Id == context.OrderStatus
									   .Where(z => z.IdOrderHead.Value == o.Id)
									   .Max(z => z.Id)
								   select new
								   {
									   Id = o.Id,
									   RoomCode = r.RoomCode,
									   FullName = r.FirstName + " " + r.LastName,
									   Description = o.Description,
									   StatusCode = s.StatusCode,
									   StatusName = sn.Name,
									   Comment = s.Description,
									   Reason = m.Description,
									   PaymType = g.Name,
									   IntakeReason = ir.Description,
									   Phone = o.Phone,
									   ProviderName = p.Name,
									   IdProvider = p.Id,
									   AtentionTime = o.AtentionTime,
									   Schedule = h.Description,
									   BreakFastDate = o.BreakFastDate,
									   LateCheckout = l.Description,
									   Total = o.Total,
									   LastUpdate = s.Created,
									   Created = o.Created,
									   Viewed = o.Viewed
								   }).FirstOrDefaultAsync();

				response.data = order;
			});
		}

		public async Task<ResponseBE> ListByClient(OrderBE orderBE)
		{
			return await GetResponse(orderBE, MyRole.Client, async (response) =>
			{
				int IdReservation = orderBE.TokenBE.Id;
				var orders = await (from o in context.OrderHead
									join s in context.OrderStatus on o.Id equals s.IdOrderHead
									join p in context.Provider on o.IdProvider equals p.Id
									join r in context.Reservation on o.IdReservation equals r.Id
									from m in context.Reason.Where(x => x.Id == s.IdReason).DefaultIfEmpty()
									from h in context.Schedule.Where(x => x.Id == o.IdSchedule).DefaultIfEmpty()
									from g in context.PaymType.Where(x => x.Id == o.IdPaymType).DefaultIfEmpty()
									from ir in context.IntakeReason.Where(x => x.Id == o.IdIntakeReason).DefaultIfEmpty()
									from l in context.LateCheckout.Where(x => x.Id == o.IdLateCheckout).DefaultIfEmpty()
									from sn in context.StatusNames.Where(x => x.Code == s.StatusCode).DefaultIfEmpty()
									where o.IdReservation == IdReservation
									&& s.Id == context.OrderStatus
										.Where(z => z.IdOrderHead.Value == o.Id)
										.Max(z => z.Id)
									orderby o.Id descending
									select new
									{
										Id = o.Id,
										RoomCode = r.RoomCode,
										FullName = r.FirstName + " " + r.LastName,
										Description = o.Description,
										StatusCode = s.StatusCode,
										StatusName = sn.Name,
										Comment = s.Description,
										Reason = m.Description,
										Phone = o.Phone,
										Gender = r.Gender,
										ProviderName = p.Name,
										IdProvider = p.Id,
										PaymType = g.Name,
										IntakeReason = ir.Description,
										AtentionTime = o.AtentionTime,
										Schedule = h.Description,
										BreakFastDate = o.BreakFastDate,
										LateCheckout = l.Description,
										Total = o.Total,
										LastUpdate = s.Created,
										Created = o.Created,
										Viewed = o.Viewed
									}).ToListAsync();

				response.data = orders;
			});
		}

		public async Task<ResponseBE> List(OrderFiltersBE orderFiltersBE)
		{
			return await GetResponse(orderFiltersBE, MyRole.Admin, async (response) =>
			{
				var ReservationFilter = (from x in context.Reservation
										 select x);

				if (!string.IsNullOrWhiteSpace(orderFiltersBE.HotelCode))
				{
					ReservationFilter = ReservationFilter
						.Where(x => x.HotelCode == orderFiltersBE.HotelCode);
				}
				if (!string.IsNullOrWhiteSpace(orderFiltersBE.FirstName))
				{
					ReservationFilter = ReservationFilter
						.Where(x => x.FirstName.StartsWith(orderFiltersBE.FirstName));
				}
				if (!string.IsNullOrWhiteSpace(orderFiltersBE.LastName))
				{
					ReservationFilter = ReservationFilter
						.Where(x => x.LastName.StartsWith(orderFiltersBE.LastName));
				}
				if (!string.IsNullOrWhiteSpace(orderFiltersBE.RoomCode))
				{
					ReservationFilter = ReservationFilter
						.Where(x => x.RoomCode == orderFiltersBE.RoomCode);
				}

				var OrderStatusFilter = (from x in context.OrderStatus
										 select x);

				if (orderFiltersBE.StatusCode == "PA")
				{
					OrderStatusFilter = OrderStatusFilter
						.Where(x => x.StatusCode == "A" || x.StatusCode == "P");
				}
				else if (orderFiltersBE.StatusCode == "T")
				{
					//DO NOTHING, NO FILTERS
				}
				else
				{
					OrderStatusFilter = OrderStatusFilter
						.Where(x => x.StatusCode == orderFiltersBE.StatusCode);
				}

				var ProviderFilter = (from x in context.Provider select x);
				if (orderFiltersBE.IdProvider != null && orderFiltersBE.IdProvider.Count > 0)
				{
					ProviderFilter = ProviderFilter.Where(x => orderFiltersBE.IdProvider.Select(c => c.id).Contains(x.Id));
				}

				int skip = orderFiltersBE.CurrentPage * orderFiltersBE.PageSize;
				int take = orderFiltersBE.PageSize;
				var query = (from o in context.OrderHead
							 join s in OrderStatusFilter on o.Id equals s.IdOrderHead
							 join r in ReservationFilter on o.IdReservation equals r.Id
							 join p in ProviderFilter on o.IdProvider equals p.Id
							 join h in context.Hotels on r.HotelCode equals h.HotelCode
							 join m in context.Permission on r.HotelCode equals m.HotelCode
							 from g in context.PaymType.Where(x => x.Id == o.IdPaymType).DefaultIfEmpty()
							 from ir in context.IntakeReason.Where(x => x.Id == o.IdIntakeReason).DefaultIfEmpty()
							 from t in context.Schedule.Where(x => x.Id == o.IdSchedule).DefaultIfEmpty()
							 from l in context.LateCheckout.Where(x => x.Id == o.IdLateCheckout).DefaultIfEmpty()
							 from sn in context.StatusNames.Where(x => x.Code == s.StatusCode).DefaultIfEmpty()
							 where m.IdUser == orderFiltersBE.TokenBE.Id
							 && s.Id == context.OrderStatus.Where(z => z.IdOrderHead.Value == o.Id).Max(z => z.Id)
							 && h.Url == context.Hotels.Where(z => z.HotelCode == r.HotelCode).Max(z => z.Url)
							 orderby o.Id descending
							 select new
							 {
								 Id = o.Id,
								 RoomCode = r.RoomCode,
								 FullName = r.FirstName + " " + r.LastName,
								 Description = o.Description,
								 Comment = s.Description,
								 StatusCode = s.StatusCode,
								 StatusName = sn.Name,
								 Phone = o.Phone,
								 Gender = r.Gender,
								 ProviderName = p.Name,
								 IdProvider = p.Id,
								 PaymType = g.Name,
								 IntakeReason = ir.Description,
								 AtentionTime = o.AtentionTime,
								 HotelCode = h.Title,
								 Schedule = t.Description,
								 LateCheckout = l.Description,
								 BreakFastDate = o.BreakFastDate,
								 Total = o.Total,
								 LastUpdate = s.Created,
								 Created = o.Created,
								 Client = $"{r.ReseYear}-{r.ReseCode}",
							 })
							 .Skip(skip)
							 .Take(take);

				var orders = await query.ToListAsync();
				response.data = orders;
			});
		}

		public async Task<ResponseBE> Detail(OrderBE orderBE)
		{
			return await GetResponse(orderBE, MyRole.Both, async (response) =>
			{
				var detail = await (from d in context.OrderDetail
									join p in context.Product on d.IdProduct equals p.Id
									join t in context.ProductType on p.IdProductType equals t.Id
									where d.IdOrderHead == orderBE.Id
									select new
									{
										Id = d.Id,
										IdProduct = p.Id,
										Name = t.Name,
										Comment = d.Description,
										Price = d.Price,
										Modifier = d.Modifiers,
										Quantity = d.Quantity,
										Image = t.Image
									}).ToListAsync();

				response.data = detail;
			});
		}

		public async Task<ResponseBE> Approve(OrderBE orderBE)
		{
			return await GetResponseTransaction(orderBE, MyRole.Admin, context, async (response) =>
			{
				var head = await context.OrderHead.FirstOrDefaultAsync(x => x.Id == orderBE.Id);
				if (head == null)
					throw new MyException($"No se encontró la orden con ID = {orderBE.Id}");

				var status = await (from s in context.OrderStatus
									where s.IdOrderHead == head.Id
									&& s.Id == context.OrderStatus
									.Where(z => z.IdOrderHead.Value == head.Id)
									.Max(z => z.Id)
									select s).FirstOrDefaultAsync();

				if (status.StatusCode != "P")
					throw new MyException($"No se puede aprobar una orden que no está pendiente.");

				OrderStatus newStatus = new OrderStatus();
				newStatus.IdOrderHead = head.Id;
				newStatus.StatusCode = "A";
				newStatus.Description = orderBE.Comment;
				newStatus.Active = true;
				newStatus.IdAppUser = orderBE.TokenBE.Id;
				newStatus.Created = DateTime.Now;

				head.AtentionTime = orderBE.AtentionTime;
				head.Viewed = false;

				await context.OrderStatus.AddAsync(newStatus);
				await context.SaveChangesAsync();
			});
		}

		public async Task<ResponseBE> Reject(OrderBE orderBE)
		{
			return await GetResponseTransaction(orderBE, MyRole.Admin, context, async (response) =>
			{
				var head = await context.OrderHead.FirstOrDefaultAsync(x => x.Id == orderBE.Id);
				if (head == null)
					throw new MyException($"No se encontró la orden con ID = {orderBE.Id}");

				var status = await (from s in context.OrderStatus
									where s.IdOrderHead == head.Id
									&& s.Id == context.OrderStatus
									.Where(z => z.IdOrderHead.Value == head.Id)
									.Max(z => z.Id)
									select s).FirstOrDefaultAsync();

				if (status.StatusCode != "P")
					throw new MyException($"No se puede rechazar una orden que no está pendiente.");

				OrderStatus newStatus = new OrderStatus();
				newStatus.IdOrderHead = head.Id;
				newStatus.IdReason = orderBE.IdReason;
				newStatus.StatusCode = "R";
				newStatus.Description = orderBE.Comment;
				newStatus.Active = true;
				newStatus.Created = DateTime.Now;
				newStatus.IdAppUser = orderBE.TokenBE.Id;

				head.Viewed = false;

				await context.OrderStatus.AddAsync(newStatus);
				await context.SaveChangesAsync();
			});
		}

		public async Task<ResponseBE> Finalize(OrderBE orderBE)
		{
			return await GetResponseTransaction(orderBE, MyRole.Admin, context, async (response) =>
			{
				var head = await context.OrderHead.FirstOrDefaultAsync(x => x.Id == orderBE.Id);
				if (head == null)
					throw new MyException($"No se encontró la orden con ID = {orderBE.Id}");

				var status = await (from s in context.OrderStatus
									where s.IdOrderHead == head.Id
									&& s.Id == context.OrderStatus
										.Where(z => z.IdOrderHead.Value == head.Id)
										.Max(z => z.Id)
									select s).FirstOrDefaultAsync();

				if (status.StatusCode != "A")
					throw new MyException($"No se puede finalizar una orden que no esté aprobada.");

				OrderStatus newStatus = new OrderStatus();
				newStatus.IdOrderHead = head.Id;
				newStatus.StatusCode = "F";
				newStatus.Description = orderBE.Comment;
				newStatus.Active = true;
				newStatus.Created = DateTime.Now;
				newStatus.IdAppUser = orderBE.TokenBE.Id;

				//head.Viewed = false;

				await context.OrderStatus.AddAsync(newStatus);
				await context.SaveChangesAsync();
			});
		}

		public async Task<ResponseBE> GetDiscount(OrderBE orderBE)
		{
			return await GetResponse(orderBE, MyRole.Client, async (response) =>
			{
				var reservation = await (from r in context.Reservation
										 where r.Active && r.Id == orderBE.TokenBE.Id
										 select r).FirstOrDefaultAsync();

				if (reservation == null)
					throw new TokenException("No se encontró la reservación.");
				
				if (reservation.Tipo == "HOME")
				{
					var promo = await (from p in context.Promo
									   join t in context.PromoType on p.IdPromoType equals t.Id
									   where
									   p.DateStart <= reservation.RegisterDate
									   && (p.DateEnd == null || p.DateEnd >= reservation.RegisterDate)
									   && p.Active
									   && p.IdPromoType == 3
									   orderby p.Discount descending
									   select new
									   {
										   Discount = p.Discount,
										   Id = p.IdPromoType,
										   Name = t.Name
									   }).FirstOrDefaultAsync();
					response.data = promo;
				}
				else
				{
					bool canGetPromoLife = !string.IsNullOrWhiteSpace(reservation.LoyaltyCode);
					bool canGetPromoWeb = reservation.MarketSegmentCode == "PAW";

					var promo = await (from p in context.Promo
									   join t in context.PromoType on p.IdPromoType equals t.Id
									   where
									   p.DateStart <= reservation.RegisterDate
									   && p.DateEnd >= reservation.RegisterDate
									   && p.Active
									   && ((canGetPromoLife && p.IdPromoType == 1) || (canGetPromoWeb && p.IdPromoType == 2))
									   orderby p.Discount descending
									   select new
									   {
										   Discount = p.Discount,
										   Id = p.IdPromoType,
										   Name = t.Name
									   }).FirstOrDefaultAsync();
					response.data = promo;
				}
			});
		}
	}
}
