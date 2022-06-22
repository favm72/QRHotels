using System;
using System.Collections.Generic;
using System.Text;

namespace SoapClient
{
	public class MyRequest
	{
		public string PublicKey { get; set; }
		public string HotelCode { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string Lastname { get; set; }
		public string RoomCode { get; set; }
	}
}
