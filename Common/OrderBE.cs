using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
	public class OrderBE : IHasToken
	{
		public string Token { get; set; }
		public TokenBE TokenBE { get; set; }

		public int Id { get; set; }
		public int IdReason { get; set; }
		public int AtentionTime { get; set; }

		public decimal Total { get; set; }
		public decimal SubTotal { get; set; }
		public int IdPaymType { get; set; }
		public int IdIntakeReason { get; set; }
		public string Phone { get; set; }
		public int IdProvider { get; set; }
		public int? IdPromo { get; set; }
		public string Comment { get; set; }
		public int IdSchedule { get; set; }
		public int IdLateCheckout { get; set; }
		public string BreakFastDateString { get; set; }
		public List<OrderDetailBE> Products { get; set; }
	}

	public class OrderFiltersBE : IHasToken
	{
		public string Token { get; set; }
		public TokenBE TokenBE { get; set; }
		public string HotelCode { get; set; }
		public int PageSize { get; set; }
		public int CurrentPage { get; set; }
		public List<OrderFilterProvider>? IdProvider { get; set; }
		public string StatusCode { get; set; }
		public string RoomCode { get; set; }
		public string LastName { get; set; }
		public string FirstName { get; set; }
	}
	public class OrderFilterProvider {

		public int id{ get; set; }
		public string description { get; set; }
	}

	public class OrderDetailBE
	{
		public int Id { get; set; }
		public string Comment { get; set; }
		public string Modifiers { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
	}
}
