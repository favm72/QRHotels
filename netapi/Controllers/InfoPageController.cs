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
	public class InfoPageController : ControllerBase
	{
		private InfoPageBL infoPageBL;
		
		public InfoPageController(MyContext context, IOptions<MyConfig> settings)
		{			
			infoPageBL = new InfoPageBL(context, settings.Value);			
		}

		[HttpPost]
		[Route("[action]")]
		public async Task<ResponseBE> List(InfoPageBE model)
		{
			model.Token = HttpContext.Request.Headers["token"];
			return await infoPageBL.List(model);
		}

		[HttpPost]
		[Route("[action]")]
		public async Task<ResponseBE> Create(InfoPageBE model)
		{
			model.Token = HttpContext.Request.Headers["token"];
			return await infoPageBL.Create(model);
		}

		[HttpPost]
		[Route("[action]")]
		public async Task<ResponseBE> Edit(InfoPageBE model)
		{
			model.Token = HttpContext.Request.Headers["token"];
			return await infoPageBL.Edit(model);
		}
	}
}
