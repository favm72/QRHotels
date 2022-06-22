using Common;
using DataLayer;
using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netapi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class OrderController : ControllerBase
	{
		private readonly ILogger<OrderController> _logger;
		private OrderBL orderBL;

		public OrderController(ILogger<OrderController> logger, MyContext context)
		{
			_logger = logger;
			orderBL = new OrderBL(context);
		}

		[HttpPost("save")]
		public async Task<ResponseBE> Save(OrderBE orderBE)
		{
			orderBE.Token = HttpContext.Request.Headers["token"];
			return await orderBL.Save(orderBE);
		}

		[HttpPost("list")]
		public async Task<ResponseBE> List(OrderFiltersBE orderFiltersBE)
		{
			orderFiltersBE.Token = HttpContext.Request.Headers["token"];
			return await orderBL.List(orderFiltersBE);
		}

		[HttpPost("listbyclient")]
		public async Task<ResponseBE> ListByClient(OrderBE orderBE)
		{
			orderBE.Token = HttpContext.Request.Headers["token"];
			return await orderBL.ListByClient(orderBE);
		}

		[HttpPost("detail")]
		public async Task<ResponseBE> Detail(OrderBE orderBE)
		{
			orderBE.Token = HttpContext.Request.Headers["token"];
			return await orderBL.Detail(orderBE);
		}

		[HttpPost("approve")]
		public async Task<ResponseBE> Approve(OrderBE orderBE)
		{
			orderBE.Token = HttpContext.Request.Headers["token"];
			return await orderBL.Approve(orderBE);
		}

		[HttpPost("reject")]
		public async Task<ResponseBE> Reject(OrderBE orderBE)
		{
			orderBE.Token = HttpContext.Request.Headers["token"];
			return await orderBL.Reject(orderBE);
		}

		[HttpPost("finalize")]
		public async Task<ResponseBE> Finalize(OrderBE orderBE)
		{
			orderBE.Token = HttpContext.Request.Headers["token"];
			return await orderBL.Finalize(orderBE);
		}

		[HttpPost("reasons")]
		public async Task<ResponseBE> Reasons(OrderBE orderBE)
		{
			orderBE.Token = HttpContext.Request.Headers["token"];
			return await orderBL.Reasons(orderBE);
		}

		[HttpPost("notifbyclient")]
		public async Task<ResponseBE> NotificationsByClient(OrderBE orderBE)
		{
			orderBE.Token = HttpContext.Request.Headers["token"];
			return await orderBL.NotificationsByClient(orderBE);
		}

		[HttpPost("markviewed")]
		public async Task<ResponseBE> MarkViewed(OrderBE orderBE)
		{
			orderBE.Token = HttpContext.Request.Headers["token"];
			return await orderBL.MarkViewed(orderBE);
		}

		[HttpPost("findbyid")]
		public async Task<ResponseBE> OrderById(OrderBE orderBE)
		{
			orderBE.Token = HttpContext.Request.Headers["token"];
			return await orderBL.FindByID(orderBE);
		}

		[HttpPost("validatebreakfast")]
		public async Task<ResponseBE> ValidateBreakFast(OrderBE orderBE)
		{
			orderBE.Token = HttpContext.Request.Headers["token"];
			return await orderBL.ValidateBreakFast(orderBE);
		}

		[HttpPost("discount")]
		public async Task<ResponseBE> GetDiscount(OrderBE orderBE)
		{
			orderBE.Token = HttpContext.Request.Headers["token"];
			return await orderBL.GetDiscount(orderBE);
		}
	}
}
