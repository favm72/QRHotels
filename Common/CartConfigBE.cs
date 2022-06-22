using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
	public class CartConfigBE : IHasToken
	{
		public string Token { get; set; }
		public int IdProvider { get; set; }
		public string HotelCode { get; set; }
		public string HourStart { get; set; }
		public string HourEnd { get; set; }
		public bool Active { get; set; }
		public TokenBE TokenBE { get; set; }
	}
}
