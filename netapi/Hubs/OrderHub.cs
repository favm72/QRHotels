using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netapi.Hubs
{
	public class OrderHub : Hub
	{
		public async Task Send(OrderHubMessage message)
		{
			await Clients.Others.SendAsync("getorder", message);
		}
	}

	public class OrderHubMessage
	{
		public int id { get; set; }
		public string client { get; set; }
		public string statusCode { get; set; }
	}
}
