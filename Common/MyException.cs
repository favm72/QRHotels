using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
	public class MyException : Exception
	{
		public MyException(string message) : base(message)
		{
			
		}
	}

	public class TokenException : Exception
	{
		public TokenException(string message) : base(message)
		{

		}
	}
}
