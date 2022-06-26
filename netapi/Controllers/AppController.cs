using DataLayer;
using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace netapi.Controllers
{
    [ApiController]
	[Route("[controller]")]
	public class AppController : ControllerBase
	{		
		private AppBL appBL;

		public AppController(MyContext context)
		{
			appBL = new AppBL(context);
		}
		
		[HttpGet()]
        [Route("[action]/{hotelCode}")]
		public async Task<object> HasPendingOrders(string hotelCode)
		{
			
			return await appBL.HasPendingOrders(hotelCode);
		}

		[HttpGet()]
		[Route("[action]")]
		public async Task<object> Hotels()
		{		
			return await appBL.Hotels();
		}
	}
}
