using System;
using System.Collections.Generic;
using System.Text;

namespace DataOracleLayer.PLSQL
{
	public class IntakeDetailQuery
    {
		public static string Value(string hotel, string ticketNumber, string cashier, string year)
		{
            return @$"
               select 
                    ve.vend_codi as nro_ticket,
                    a.artg_Desc as producto, 
                    g.grup_desc,
                    f.fami_desc, 
                    sum(arve_totl) venta,
                    sum(arve_desc) dscto,
                    sum(arve_reca) recargo 
                from 
                    inf.tnpo_arve@{hotel} ve, 
                    inf.tnpo_artg@{hotel} a, 
                    inf.tnpo_grup@{hotel} g , 
                    inf.tnpo_fami@{hotel} f
                where 
                    ve.artg_codi = a.artg_codi  
                    and a.grup_codi = g.grup_codi  
                    and a.fami_codi = f.fami_codi 
                    and ve.vend_codi = {ticketNumber}
                    and vend_anci = {year}
                    and VE.CAJA_CODI  = {cashier}
                    and arve_anul = 0
                group by 
                    ve.vend_codi,
                    a.artg_Desc,
                    g.grup_desc,
                    f.fami_desc
            ";
        }
	}
}
