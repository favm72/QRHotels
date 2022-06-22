using Common;
using DataLayer;
using LogicLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Security;
using SoapClient;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Testing
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public async Task FindReservation()
		{
			ReservationCL reservation = new ReservationCL();
			//var response = await reservation.Find("SHINSATO", "1801");
		}

		[TestMethod]
		async public Task Logic()
		{
			var context = new MyContext();
			var infoBL = new InfoBL(context);

			TokenBE token = new TokenBE();
			token.Id = 1;
			token.Expiration = DateTime.Now.AddMinutes(20);
			token.IsAdmin = false;
			DummyBE dummyBE = new DummyBE();
			dummyBE.Token = MyCrypt.Encrypt(JsonConvert.SerializeObject(token));
			try
			{
				var res = await infoBL.HotelInfo(dummyBE);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		[TestMethod]
		public void Encrypt()
		{
			string pass = "@adrs0601@";
			var encrypted = MyCrypt.Encrypt(pass);
			var decrypted = MyCrypt.Decrypt(encrypted);
		}

		[TestMethod]
		public void Compare()
		{
			int value = string.Compare("08:00", "09:00");
			value = string.Compare("08:00", "08:01");
			value = string.Compare("08:01", "08:00");
			value = string.Compare("10:00", "10:00");
			value = string.Compare("11:00", "10:59");
		}
	}
}
