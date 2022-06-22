using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
	public class UserLogBE : IHasToken
	{
		public string Action { get; set; }
		public string Token { get; set; }
		public TokenBE TokenBE { get; set; }
	}
}
