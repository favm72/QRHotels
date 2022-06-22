using Common;
using DataLayer;
using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netapi.Controllers
{
	[ApiController]	
	[Route("[controller]")]
	public class InfoController : ControllerBase
	{
		private readonly ILogger<InfoController> _logger;
		private InfoBL infoBL;
		private IConfiguration configuration;

		public InfoController(ILogger<InfoController> logger, MyContext context, IConfiguration configuration)
		{
			_logger = logger;
			infoBL = new InfoBL(context);
			this.configuration = configuration;
		}

		[HttpPost("directory")]
		public async Task<ResponseBE> Directoy(DummyBE dummyBE)
		{
			dummyBE.Token = HttpContext.Request.Headers["token"];
			return await infoBL.DirectoryServices(dummyBE);
		}

		[HttpPost("slider")]
		public async Task<ResponseBE> Slider(DummyBE dummyBE)
		{
			dummyBE.Token = HttpContext.Request.Headers["token"];
			return await infoBL.Slider(dummyBE);
		}

		[HttpPost("schedule")]
		public async Task<ResponseBE> Schedule(DummyBE dummyBE)
		{
			dummyBE.Token = HttpContext.Request.Headers["token"];
			return await infoBL.Schedule(dummyBE);
		}

		[HttpPost("hotels")]
		public async Task<ResponseBE> Hotels(DummyBE dummyBE)
		{
			dummyBE.Token = HttpContext.Request.Headers["token"];
			return await infoBL.Hotels(dummyBE);
		}

		[HttpPost("hotelinfo")]
		public async Task<ResponseBE> HotelInfo(DummyBE dummyBE)
		{
			dummyBE.Token = HttpContext.Request.Headers["token"];
			return await infoBL.HotelInfo(dummyBE);
		}

		[HttpPost("cleaning")]
		public async Task<ResponseBE> Cleaning(DummyBE dummyBE)
		{
			dummyBE.Token = HttpContext.Request.Headers["token"];
			return await infoBL.Cleaning(dummyBE);
		}

		[HttpPost("providers")]
		public async Task<ResponseBE> Providers(DummyBE dummyBE)
		{
			dummyBE.Token = HttpContext.Request.Headers["token"];
			return await infoBL.Providers(dummyBE);
		}

		[HttpPost("latecheckout")]
		public async Task<ResponseBE> Latecheckout(DummyBE dummyBE)
		{
			dummyBE.Token = HttpContext.Request.Headers["token"];
			return await infoBL.LateCheckout(dummyBE);
		}

		[HttpPost("HotelCode")]
		public async Task<ResponseBE> HotelCode(DummyBE dummyBE)
		{
			dummyBE.Token = HttpContext.Request.Headers["token"];
			return await infoBL.HotelCode(dummyBE);
		}

		[HttpPost("paymtypes")]
		public async Task<ResponseBE> PaymTypes(DummyBE dummyBE)
		{
			dummyBE.Token = HttpContext.Request.Headers["token"];
			string conn_ora = "";
			try
			{
				conn_ora = configuration.GetConnectionString("conn_ora");
			}
			catch (Exception)
			{				
			}
			return await infoBL.PaymTypes(dummyBE, conn_ora);
		}
	}
}
