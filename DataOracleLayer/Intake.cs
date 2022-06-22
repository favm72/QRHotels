using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DataOracleLayer
{
	public class Intake
	{
		public string FechaConsumo { get; set; }
		public string ReseCodi { get; set; }
		public string TipoCta { get; set; }
		public string Seccion { get; set; }
		public string Servicio { get; set; }
		public string Moneda { get; set; }
		public decimal? Importe { get; set; }
		public decimal? ImporteSol { get; set; }
		public string Ticket { get; set; }
		public string PtoVta { get; set; }

		public string Currency {
			get
			{
				return Moneda == "USD" ? (ImporteSol > Importe ? "SOL" : "USD") : "SOL";
			}
		}
		public decimal? Amount
		{
			get
			{
				return Moneda == "USD" ? (ImporteSol > Importe ? ImporteSol : Importe) : Importe;
			}
		}
	}
}
