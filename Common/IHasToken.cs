using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
	public interface IHasToken
	{
		public string Token { get; set; }
		public TokenBE TokenBE { get; set; }
	}

	public class DummyBE : IHasToken
	{
		public string Token { get; set; }
		public TokenBE TokenBE { get; set; }
	}

	public enum MyRole
	{
		Admin,
		Client,
		Both
	}
}
