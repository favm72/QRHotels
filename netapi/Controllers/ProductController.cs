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
	public class ProductController : ControllerBase
	{	
		private ProductBL productBL;
		public ProductController(MyContext context, IOptions<MyConfig> settings)
		{			
			productBL = new ProductBL(context, settings.Value.SavePath, settings.Value.EndPoint);
		}

		[HttpPost("List")]
		public async Task<ResponseBE> List(ProductBE productBE)
		{
			productBE.Token = HttpContext.Request.Headers["token"];
			return await productBL.List(productBE);
		}

		[HttpPost("manage")]
		public async Task<ResponseBE> Manage(DummyBE dummyBE)
		{
			dummyBE.Token = HttpContext.Request.Headers["token"];
			return await productBL.Manage(dummyBE);
		}

		[HttpPost("categories")]
		public async Task<ResponseBE> Categories(ProductBE productBE)
		{
			productBE.Token = HttpContext.Request.Headers["token"];
			return await productBL.Categories(productBE);
		}
		[HttpPost("allcategories")]
		public async Task<ResponseBE> AllCategories(ProductBE productBE)
		{
			productBE.Token = HttpContext.Request.Headers["token"];
			return await productBL.CategoriesAll(productBE);
		}
		[HttpPost("search")]
		public async Task<ResponseBE> Search(ProductBE filterBE)
		{
			filterBE.Token = HttpContext.Request.Headers["token"];
			return await productBL.Search(filterBE);
		}
		[HttpPost("findbyid")]
		public async Task<ResponseBE> FindById(ProductBE filterBE)
		{
			filterBE.Token = HttpContext.Request.Headers["token"];
			return await productBL.FindById(filterBE);
		}
		[HttpPost("create")]
		public async Task<ResponseBE> Create(ProductBE productBE)
		{
			productBE.Token = HttpContext.Request.Headers["token"];
			return await productBL.Create(productBE);
		}
		[HttpPost("update")]
		public async Task<ResponseBE> Update(ProductBE productBE)
		{
			productBE.Token = HttpContext.Request.Headers["token"];
			return await productBL.Update(productBE);
		}
		[HttpPost("delete")]
		public async Task<ResponseBE> Delete(ProductBE productBE)
		{
			productBE.Token = HttpContext.Request.Headers["token"];
			return await productBL.Delete(productBE);
		}
		[HttpPost("toggle")]
		public async Task<ResponseBE> Toggle(ProductBE productBE)
		{
			productBE.Token = HttpContext.Request.Headers["token"];
			return await productBL.Toggle(productBE);
		}

		[HttpPost("cartenabled")]
		public async Task<ResponseBE> CartEnabled(ProductBE productBE)
		{
			productBE.Token = HttpContext.Request.Headers["token"];
			return await productBL.CartEnabled(productBE);
		}
	}
}
