using System;
using System.Collections.Generic;
using System.Text;

namespace DataOracleLayer.PLSQL
{
	public class CreditInfoQuery
	{
		public static string Value(string hotel, string reseCode, string year)
		{
			return @$"
				select dcre_licr as valor_credito,
				decode(dcre_unli,1,'CREDITO ILIMATADO','SIN CREDITO') as tipo_credito, 
				unmo_codi moneda_rsva
				from rmc.tnht_dcre@{hotel} 
				where rese_codi = {reseCode} 
				and rese_anci = {year}
				and movi_tico = 2
			";
		}
	}
}
