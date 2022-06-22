using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DataOracleLayer
{
	public class IntakeBE
	{
		public string IntakeDate { get; set; }
		public string ReseCode { get; set; }
		public string AccountType { get; set; }
		public string Section { get; set; }
		public string Service { get; set; }
		public string Currency { get; set; }
		public decimal? Amount { get; set; }
		public string Ticket { get; set; }
		public string PtoVta { get; set; }
		public string TicketNo
		{
			get
			{
				Match match = Regex.Match(Ticket, "^([0-9]+)A([0-9]{2})C([0-9]{1,2})$");
				return match.Success ? match.Groups[1].Value : "";
			}
		}
		public string Year
		{
			get
			{
				Match match = Regex.Match(Ticket, "^([0-9]+)A([0-9]{2})C([0-9]{1,2})$");
				return match.Success ? $"20{match.Groups[2].Value}" : "";
			}
		}
		public string Cashier
		{
			get
			{
				Match match = Regex.Match(Ticket, "^([0-9]+)A([0-9]{2})C([0-9]{1,2})$");
				return match.Success ? match.Groups[3].Value : "";
			}
		}
	}

	public class GroupIntakeBE
	{
		public string Ticket { get; set; }
		public string TicketNo
		{
			get
			{
				Match match = Regex.Match(Ticket, "^([0-9]+)A([0-9]{2})C([0-9]{1,2})$");
				return match.Success ? match.Groups[1].Value : "";
			}
		}
		public string Year
		{
			get
			{
				Match match = Regex.Match(Ticket, "^([0-9]+)A([0-9]{2})C([0-9]{1,2})$");
				return match.Success ? $"20{match.Groups[2].Value}" : "";
			}
		}
		public string Cashier
		{
			get
			{
				Match match = Regex.Match(Ticket, "^([0-9]+)A([0-9]{2})C([0-9]{1,2})$");
				return match.Success ? match.Groups[3].Value : "";
			}
		}
		public string Currency { get; set; }
		public decimal? Total { get; set; }
		public List<IntakeBE> Items { get; set; }
	}
}
