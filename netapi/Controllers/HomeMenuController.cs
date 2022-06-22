using Common;
using DataLayer;
using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace netapi.Controllers
{
    [ApiController]	
	[Route("[controller]")]
	public class HomeMenuController : ControllerBase
	{
		private HomeMenuBL homeMenuBL;
		
		public HomeMenuController(MyContext context, IOptions<MyConfig> settings)
		{			
			homeMenuBL = new HomeMenuBL(context, settings.Value);			
		}

		[HttpPost]
		[Route("[action]")]
		public async Task<ResponseBE> List(HomeMenuBE model)
		{
			model.Token = HttpContext.Request.Headers["token"];
			return await homeMenuBL.List(model);
		}

		[HttpPost]
		[Route("[action]")]
		public async Task<ResponseBE> ListAdmin(HomeMenuBE model)
		{
			model.Token = HttpContext.Request.Headers["token"];
			return await homeMenuBL.ListAdmin(model);
		}

		[HttpPost]
		[Route("[action]")]
		public async Task<ResponseBE> Create(HomeMenuBE model)
		{
			model.Token = HttpContext.Request.Headers["token"];
			return await homeMenuBL.Create(model);
		}

		[HttpPost]
		[Route("[action]")]
		public async Task<ResponseBE> Edit(HomeMenuBE model)
		{
			model.Token = HttpContext.Request.Headers["token"];
			return await homeMenuBL.Edit(model);
		}
	}
}
