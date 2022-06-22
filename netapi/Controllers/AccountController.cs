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
	public class AccountController : ControllerBase
	{
		private readonly ILogger<AccountController> _logger;
		private AccountBL accountBL;

		public AccountController(ILogger<AccountController> logger, MyContext context, IOptions<MyConfig> settings)
		{
			_logger = logger;
			accountBL = new AccountBL(context, settings.Value);
		}

		[HttpPost("login")]
		public async Task<ResponseBE> Login(AccountBE account)
		{
			return await accountBL.Login(account);
		}

		[HttpPost("hotel")]
		public async Task<ResponseBE> Hotal(AccountBE account)
		{
			return await accountBL.GetHotel(account.HotelCode);
		}

		[HttpPost("admin/login")]
		public async Task<ResponseBE> AdminLogin(AccountBE account)
		{
			return await accountBL.AdminLogin(account);
		}
	}
}
