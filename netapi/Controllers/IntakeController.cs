using Common;
using DataLayer;
using DataOracleLayer;
using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace netapi.Controllers
{
	[ApiController]	
	[Route("[controller]")]
	public class IntakeController : ControllerBase
	{
		private readonly ILogger<IntakeController> _logger;
		private IntakeBL intakeBL;

		public IntakeController(
			ILogger<IntakeController> logger, 
			MyContext context,
			IOptions<MyConfig> settings,
			IConfiguration configuration)
		{
			_logger = logger;
			OracleContext oracleContext = new OracleContext(configuration.GetConnectionString("conn_ora"));
			intakeBL = new IntakeBL(context, oracleContext, settings.Value);
		}

		[HttpPost("List")]
		public async Task<ResponseBE> List(DummyBE dummyBE)
		{
			dummyBE.Token = HttpContext.Request.Headers["token"];
			return await intakeBL.List(dummyBE);
		}

		[HttpPost("Detail")]
		public async Task<ResponseBE> Detail(IntakeDetailBE intakeDetailBE)
		{
			intakeDetailBE.Token = HttpContext.Request.Headers["token"];
			return await intakeBL.Detail(intakeDetailBE);
		}

		[HttpPost("Reasons")]
		public async Task<ResponseBE> Reasons(DummyBE dummyBE)
		{
			dummyBE.Token = HttpContext.Request.Headers["token"];
			return await intakeBL.Reasons(dummyBE);
		}

		[HttpPost("SendMail")]
		public async Task<ResponseBE> SendMail(IntakeMailBE intakeMailBE)
		{
			intakeMailBE.Token = HttpContext.Request.Headers["token"];
			return await intakeBL.SendEmail(intakeMailBE);
		}

		[HttpPost("CanAgree")]
		public async Task<ResponseBE> CanAgree(DummyBE dummyBE)
		{
			dummyBE.Token = HttpContext.Request.Headers["token"];
			return await intakeBL.CanAgree(dummyBE);
		}
	}
}
