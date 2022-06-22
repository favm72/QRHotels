using Common;
using LogicLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Security;
using SoapClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Testing
{
	[TestClass]
	public class BreakFastTest
	{
		OrderBL orderBL;
		OrderBE orderBE;
		public BreakFastTest()
		{
			orderBL = new OrderBL(null);
			orderBE = new OrderBE();
			orderBE.IdSchedule = 1;
			orderBE.Products = new List<OrderDetailBE>() { new OrderDetailBE() };
		}

		[TestMethod]
		public async Task InvalidDate()
		{
			orderBE.BreakFastDateString = "32-02-2021";
			Func<Task<string>> dummy1 = async () => { return "08:00"; };
			Func<Task<int?>> dummy2 = async () => { return 5; };
			bool ex = false;
			try
			{
				var date = await orderBL.ValidateBreakfastDate(orderBE, dummy1, dummy2);
			}
			catch (Exception)
			{
				ex = true;
			}
			Assert.IsTrue(ex);
		}

		[TestMethod]
		public async Task UnselectedSchedule()
		{
			orderBE.BreakFastDateString = "01-02-2021";
			orderBE.IdSchedule = 0;
			Func<Task<string>> dummy1 = async () => { return "08:00"; };
			Func<Task<int?>> dummy2 = async () => { return 5; };
			bool ex = false;
			try
			{
				var date = await orderBL.ValidateBreakfastDate(orderBE, dummy1, dummy2);
			}
			catch (Exception)
			{
				ex = true;
			}
			Assert.IsTrue(ex);
		}

		[TestMethod]
		public async Task PrevDate()
		{
			orderBE.BreakFastDateString = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd");
			orderBE.IdSchedule = 1;
			Func<Task<string>> dummy1 = async () => { return "08:00"; };
			Func<Task<int?>> dummy2 = async () => { return 5; };
			bool ex = false;
			try
			{
				var date = await orderBL.ValidateBreakfastDate(orderBE, dummy1, dummy2);
			}
			catch (Exception)
			{
				ex = true;
			}
			Assert.IsTrue(ex);
		}

		[TestMethod]
		public async Task SameDayPrevHour()
		{
			orderBE.BreakFastDateString = DateTime.Now.ToString("yyyy-MM-dd");
			orderBE.IdSchedule = 1;
			Func<Task<string>> dummy1 = async () => { return DateTime.Now.AddHours(-1).ToString("HH:mm"); };
			Func<Task<int?>> dummy2 = async () => { return 5; };
			bool ex = false;
			try
			{
				var date = await orderBL.ValidateBreakfastDate(orderBE, dummy1, dummy2);
			}
			catch (Exception)
			{
				ex = true;
			}
			Assert.IsTrue(ex);
		}

		[TestMethod]
		public async Task SameDayNotEnoughMinutes()
		{
			orderBE.BreakFastDateString = DateTime.Now.ToString("yyyy-MM-dd");
			orderBE.IdSchedule = 1;
			Func<Task<string>> dummy1 = async () => { return DateTime.Now.AddMinutes(20).ToString("HH:mm"); };
			Func<Task<int?>> dummy2 = async () => { return 40; };
			bool ex = false;
			try
			{
				var date = await orderBL.ValidateBreakfastDate(orderBE, dummy1, dummy2);
			}
			catch (Exception)
			{
				ex = true;
			}
			Assert.IsTrue(ex);
		}

		[TestMethod]
		public async Task SameDayEnoughMinutes()
		{
			orderBE.BreakFastDateString = DateTime.Now.ToString("yyyy-MM-dd");
			orderBE.IdSchedule = 1;
			Func<Task<string>> dummy1 = async () => { return DateTime.Now.AddMinutes(50).ToString("HH:mm"); };
			Func<Task<int?>> dummy2 = async () => { return 40; };
			bool ex = false;
			try
			{
				var date = await orderBL.ValidateBreakfastDate(orderBE, dummy1, dummy2);
			}
			catch (Exception)
			{
				ex = true;
			}
			Assert.IsFalse(ex);
		}

		[TestMethod]
		public async Task NextDay()
		{
			orderBE.BreakFastDateString = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
			orderBE.IdSchedule = 1;
			Func<Task<string>> dummy1 = async () => { return "07:00"; };
			Func<Task<int?>> dummy2 = async () => { return 40; };
			bool ex = false;
			try
			{
				var date = await orderBL.ValidateBreakfastDate(orderBE, dummy1, dummy2);
			}
			catch (Exception)
			{
				ex = true;
			}
			Assert.IsFalse(ex);
		}
	}
}
