using Common;
using DataLayer;
using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netapi.Controllers
{
	[ApiController]	
	[Route("[controller]")]
	public class CartConfigController : ControllerBase
	{
		private readonly ILogger<CartConfigController> _logger;
		private CartConfigBL cartConfigBL;

		public CartConfigController(ILogger<CartConfigController> logger, MyContext context)
		{
			_logger = logger;
			cartConfigBL = new CartConfigBL(context);
		}

		[HttpPost("list")]
		public async Task<ResponseBE> List(DummyBE dummyBE)
		{
			dummyBE.Token = HttpContext.Request.Headers["token"];
			return await cartConfigBL.List(dummyBE);
		}

		[HttpPost("add")]
		public async Task<ResponseBE> Add(CartConfigBE cartConfigBE)
		{
			cartConfigBE.Token = HttpContext.Request.Headers["token"];
			return await cartConfigBL.Add(cartConfigBE);
		}

		[HttpPost("edit")]
		public async Task<ResponseBE> Edit(CartConfigBE cartConfigBE)
		{
			cartConfigBE.Token = HttpContext.Request.Headers["token"];
			return await cartConfigBL.Edit(cartConfigBE);
		}

	}
}
