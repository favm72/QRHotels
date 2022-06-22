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
	public class UserLogController : ControllerBase
	{
		private readonly ILogger<UserLogController> _logger;
		private UserLogBL userLogBL;

		public UserLogController(ILogger<UserLogController> logger, MyContext context)
		{
			_logger = logger;
			userLogBL = new UserLogBL(context);
		}

		[HttpPost("UserLog")]
		public async Task<ResponseBE> Log(UserLogBE log)
		{
			log.Token = HttpContext.Request.Headers["token"];
			return await userLogBL.Insert(log);
		}
	}
}
