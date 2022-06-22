using System;
using System.Collections.Generic;
using System.Text;

namespace DataOracleLayer.PLSQL
{
	public class IntakeListQuery
	{
		public static string Value(string hotel, string reseCode, string reseYear)
		{
            return @$"
                Select 
                    m.movi_dava as fecha_consumo,
                    m.rese_codi,
                    m.movi_tico tipo_cta,
                    secc.secc_desc,
                    serv_desc || decode(nvl(movi_pove,0),0,decode(movi_desc,null,'',':'|| movi_desc)) as servicio,
                    m.unmo_codi mon,
                    sum(m.movi_vcre) as importe,
                    sum(m.movi_vame) as importe_sol,
                    nvl(m.movi_nudo, m.movi_desc) as nro_ticket, 
                    m.movi_pove as  pto_vta,
                    m.movi_orig
                From 
                    rmc.tnht_movi@{hotel} m, 
                    rmc.tnht_secc@{hotel} secc, 
                    rmc.tnht_serv@{hotel} serv, 
                    rmc.tnht_rese@{hotel} r  
                Where 
                    r.rres_codi = {reseCode} 
                    and r.rese_anci = {reseYear} 
                    and m.serv_codi = serv.serv_codi 
                    and m.rese_codi = r.rese_codi 
                    and m.rese_anci = r.rese_anci 
                    and m.secc_codi = secc.secc_codi 
                    and m.movi_anul = 0 
                    and m.movi_corr = 0 
                    and m.sefa_CODI is null 
                    and m.movi_timo = 1
                    and movi_tico not in (1,6)
                    --and m.seCC_codi <> 'ALO' 
                    --and m.SERV_CODI <> 'DES'
                group by 
                    m.movi_dava,
                    m.rese_codi,
                    m.movi_tico,
                    secc.secc_desc,
                    serv.serv_desc,
                    m.unmo_codi,
                    m.movi_nudo,
                    m.movi_desc,
                    m.movi_pove,
                    m.movi_orig
                order by m.movi_dava, 9, secc.secc_desc, serv.serv_desc
            ";
        }
	}
}
