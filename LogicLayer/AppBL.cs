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
	public class AppBL : BaseBL
	{
		private MyContext context;
		public AppBL(MyContext context)
		{
			this.context = context;
		}

		public async Task<object> HasPendingOrders(string hotelCode)
        {
			bool result =
			await (from o in context.OrderHead
				   join r in context.Reservation on o.IdReservation equals r.Id
				   join s in context.OrderStatus on o.Id equals s.IdOrderHead
				   where s.Id == context.OrderStatus.Where(z => z.IdOrderHead.Value == o.Id).Max(z => z.Id)
				   && s.StatusCode == "P"
				   && r.HotelCode == hotelCode
				   select 1).AnyAsync();
			return new { Result = result };
        }


		public async Task<object> Hotels()
		{
			return await (from h in context.Hotels
						  orderby h.ShortDescription descending
						  select new
						  {
							  HotelCode = h.HotelCode,
							  Description = h.ShortDescription
						  }).ToListAsync();
		}	
	}
}
