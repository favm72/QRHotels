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
	public class DigitalMenuBL : BaseBL
	{
		private MyContext context { get; set; }
		public DigitalMenuBL(MyContext context)
		{
			this.context = context;
		}
		public async Task<ResponseBE> List(DummyBE dummyBE)
		{
			return await GetResponse(dummyBE, MyRole.Client, async (response) =>
			{
				var reservation = await (from r in context.Reservation
										 where r.Active && r.Id == dummyBE.TokenBE.Id
										 select r).FirstOrDefaultAsync();

				var list = await (from d in context.DigitalMenu
								  where d.HotelCode == reservation.HotelCode
								  && d.Active
								  orderby d.OrderNo
								  select new
								  {
									  d.IdProvider,
									  d.Image,
									  d.IsCart,
									  d.ViewOnly
								  }).ToListAsync();

				response.data = list;
			});
		}
	}
}
