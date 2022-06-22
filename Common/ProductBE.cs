using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
	public class ProductBE : IHasToken
	{
		public string Token { get; set; }
		public int IdProvider { get; set; }
		public int Id { get; set; }
		public TokenBE TokenBE { get; set; }

		public int IdCategory { get; set; }
		public string HotelCode { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
		public decimal Price { get; set; }
		public int OrderNo { get; set; }
		public bool Active { get; set; }
		public string Modifiers { get; set; }
		public string FileName { get; set; }
		public bool newCategory { get; set; }
		public string categoryName { get; set; }
	}
}
