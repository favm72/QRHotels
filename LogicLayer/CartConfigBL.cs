using Common;
using SoapClient;
using DataLayer;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace LogicLayer
{
	public class CartConfigBL : BaseBL
	{
		private MyContext context { get; set; }
		public CartConfigBL(MyContext context)
		{
			this.context = context;
		}
		public async Task<ResponseBE> List(DummyBE dummyBE)
		{
			return await GetResponse(dummyBE, MyRole.Admin, async (response) =>
			{
				var list = await (from c in context.CartConfig
								  join p in context.Provider on c.IdProvider equals p.Id
								  join h in context.Hotels on c.HotelCode equals h.HotelCode
								  select new
								  {
									  c.IdProvider,
									  Provider = p.Name,
									  Hotel = h.ShortDescription,
									  c.HotelCode,
									  c.HourStart,
									  c.HourEnd,
									  c.Active
								  }).ToListAsync();

				response.data = list;
			});
		}

		public void validate(CartConfigBE cartConfigBE)
		{
			if (string.IsNullOrWhiteSpace(cartConfigBE.HotelCode))
				throw new MyException("Seleccione el Hotel");
			if (cartConfigBE.IdProvider == 0)
				throw new MyException("Seleccione el Carrito");

			Func<string, bool> validHour = s => Regex.IsMatch(s, "^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$");
			if (!validHour(cartConfigBE.HourStart))
				throw new MyException("Hora de INICIO no válida.");
			if (!validHour(cartConfigBE.HourEnd))
				throw new MyException("Hora de FIN no válida.");
			if (string.Compare(cartConfigBE.HourStart,cartConfigBE.HourEnd) != -1)
				throw new MyException("La hora de INICIO debe ser menor que la hora de FIN.");
		}

		public async Task<ResponseBE> Add(CartConfigBE cartConfigBE)
		{
			return await GetResponseTransaction(cartConfigBE, MyRole.Admin, context, async (response) =>
			{
				validate(cartConfigBE);
				var exists = await (from c in context.CartConfig
									where c.IdProvider == cartConfigBE.IdProvider
									&& c.HotelCode == cartConfigBE.HotelCode
									select c).AnyAsync();
				if (exists)
					throw new MyException("Ya existe una configuración para el carrito y hotel seleccionados.");

				var toAdd = new CartConfig();
				toAdd.Active = cartConfigBE.Active;
				toAdd.HotelCode = cartConfigBE.HotelCode;
				toAdd.HourStart = cartConfigBE.HourStart;
				toAdd.HourEnd = cartConfigBE.HourEnd;
				toAdd.IdProvider = cartConfigBE.IdProvider;
				await context.CartConfig.AddAsync(toAdd);
				await context.SaveChangesAsync();
			});
		}

		public async Task<ResponseBE> Edit(CartConfigBE cartConfigBE)
		{
			return await GetResponseTransaction(cartConfigBE, MyRole.Admin, context, async (response) =>
			{
				validate(cartConfigBE);

				var toEdit = await (from c in context.CartConfig
									where c.IdProvider == cartConfigBE.IdProvider
									&& c.HotelCode == cartConfigBE.HotelCode
									select c).FirstOrDefaultAsync();

				toEdit.HourStart = cartConfigBE.HourStart;
				toEdit.HourEnd = cartConfigBE.HourEnd;
				toEdit.Active = cartConfigBE.Active;

				await context.SaveChangesAsync();
			});
		}
	}
}
