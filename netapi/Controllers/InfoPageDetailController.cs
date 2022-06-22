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
	public class InfoPageDetailController : ControllerBase
	{
		private InfoPageDetailBL infoPageDetailBL;
		
		public InfoPageDetailController(MyContext context, IOptions<MyConfig> settings)
		{			
			infoPageDetailBL = new InfoPageDetailBL(context, settings.Value);			
		}

		[HttpPost]
		[Route("[action]")]
		public async Task<ResponseBE> List(InfoPageDetailBE model)
		{
			model.Token = HttpContext.Request.Headers["token"];
			return await infoPageDetailBL.List(model);
		}

		[HttpPost]
		[Route("[action]")]
		public async Task<ResponseBE> ListAdmin(InfoPageDetailBE model)
		{
			model.Token = HttpContext.Request.Headers["token"];
			return await infoPageDetailBL.ListAdmin(model);
		}

		[HttpPost]
		[Route("[action]")]
		public async Task<ResponseBE> Create(InfoPageDetailBE model)
		{
			model.Token = HttpContext.Request.Headers["token"];
			return await infoPageDetailBL.Create(model);
		}

		[HttpPost]
		[Route("[action]")]
		public async Task<ResponseBE> Edit(InfoPageDetailBE model)
		{
			model.Token = HttpContext.Request.Headers["token"];
			return await infoPageDetailBL.Edit(model);
		}
	}
}
