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
	public class DirectoryController : ControllerBase
	{
		private DirectoryBL directoryBL;
		
		public DirectoryController(MyContext context, IOptions<MyConfig> settings)
		{			
			directoryBL = new DirectoryBL(context, settings.Value);			
		}

		[HttpPost]
		[Route("[action]")]
		public async Task<ResponseBE> List(DirectoryBE model)
		{
			model.Token = HttpContext.Request.Headers["token"];
			return await directoryBL.List(model);
		}

		[HttpPost]
		[Route("[action]")]
		public async Task<ResponseBE> ListAdmin(DirectoryBE model)
		{
			model.Token = HttpContext.Request.Headers["token"];
			return await directoryBL.ListAdmin(model);
		}

		[HttpPost]
		[Route("[action]")]
		public async Task<ResponseBE> UpsertHead(DirectoryBE model)
		{
			model.Token = HttpContext.Request.Headers["token"];
			return await directoryBL.UpsertHead(model);
		}

		[HttpPost]
		[Route("[action]")]
		public async Task<ResponseBE> Create(DirectoryBE model)
		{
			model.Token = HttpContext.Request.Headers["token"];
			return await directoryBL.Create(model);
		}

		[HttpPost]
		[Route("[action]")]
		public async Task<ResponseBE> Edit(DirectoryBE model)
		{
			model.Token = HttpContext.Request.Headers["token"];
			return await directoryBL.Edit(model);
		}
	}
}
