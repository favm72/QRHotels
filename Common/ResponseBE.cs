using System;

namespace Common
{
	public class ResponseBE
	{
		public bool status { get; set; }
		public string message { get; set; }
		public object data { get; set; }
		public bool logout { get; set; }
	}
}
