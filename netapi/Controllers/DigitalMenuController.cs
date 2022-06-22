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
	public class DigitalMenuController : ControllerBase
	{
		private readonly ILogger<DigitalMenuController> _logger;
		private DigitalMenuBL digitalMenuBL;

		public DigitalMenuController(ILogger<DigitalMenuController> logger, MyContext context)
		{
			_logger = logger;
			digitalMenuBL = new DigitalMenuBL(context);
		}

		[HttpPost("list")]
		public async Task<ResponseBE> List(DummyBE dummyBE)
		{
			dummyBE.Token = HttpContext.Request.Headers["token"];
			return await digitalMenuBL.List(dummyBE);
		}
	}
}
