using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
	public class IntakeDetailBE : IHasToken
	{
		public string Token { get; set; }
		public TokenBE TokenBE { get; set; }
		public string Ticket { get; set; }
	}
}
