using Common;
using SoapClient;
using DataLayer;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LogicLayer
{
	public class UserLogBL : BaseBL
	{
		private MyContext context { get; set; }
		public UserLogBL(MyContext context)
		{
			this.context = context;
		}

		public async Task<ResponseBE> Insert(UserLogBE log)
		{
			return await GetResponse(log, MyRole.Client, async (response) => {
				UserLog userLog = new UserLog();
				userLog.Action = log.Action;
				userLog.Created = DateTime.Now;
				userLog.IdReservation = log.TokenBE.Id;
				await context.UserLog.AddAsync(userLog);
				await context.SaveChangesAsync();
			});
		}
	}
}
