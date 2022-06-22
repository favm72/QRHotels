using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DataOracleLayer
{
	public class IntakeDetail
	{
		public string NroTicket { get; set; }
		public string Producto { get; set; }
		public string GroupDesc { get; set; }
		public string FamiDesc { get; set; }
		public decimal? Venta { get; set; }
		public decimal? Dscto { get; set; }
		public decimal? Recargo { get; set; }
	}
}
