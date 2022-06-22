using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
	public class TokenBE
	{
		public int Id { get; set; }
		public bool IsAdmin { get; set; }
		public DateTime Expiration { get; set; }
	}
}
