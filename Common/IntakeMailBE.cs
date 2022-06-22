using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
	public class IntakeMailBE : IHasToken
	{
		public string Token { get; set; }
		public TokenBE TokenBE { get; set; }
		public string Description { get; set; }
		public int ReasonId { get; set; }
	}
}
