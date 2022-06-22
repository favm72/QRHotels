using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataOracleLayer
{
	public class OracleContext : IServiceProvider
	{
		private string connectionString;
		public OracleContext(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public async Task Query(string commandText, Action<OracleCommand> setParams, Action<OracleDataReader> onRead)
		{
			using (OracleConnection connection = new OracleConnection(connectionString))
			using (OracleCommand command = new OracleCommand())
			{
				command.Connection = connection;
				command.CommandType = System.Data.CommandType.Text;
				command.CommandText = commandText;
				setParams?.Invoke(command);
				await connection.OpenAsync();
				using (OracleDataReader reader = command.ExecuteReader())
				{
					if (reader.HasRows)
					{
						while (reader.Read())
						{
							onRead(reader);
						}
					}
				}
			}
		}

		public async Task<bool> isCreditUnlimited(string hotel, string reseCode, string year)
		{
			bool result = false;
			try
			{
				string tipo = "";
				await Query(PLSQL.CreditInfoQuery.Value(hotel, reseCode, year), null, r =>
				{
					tipo = r["tipo_credito"].ToString();
				});
				result = tipo == "CREDITO ILIMATADO";
			}
			catch (Exception)
			{

			}
			return result;
		}

		public async Task<List<Intake>> IntakeList(string hotel, string reseCode, string year)
		{
			List<Intake> result = new List<Intake>();
			await Query(PLSQL.IntakeListQuery.Value(hotel, reseCode, year), null, r =>
			{
				var toAdd = new Intake();
				if (!r.IsDBNull(r.GetOrdinal("fecha_consumo")))
					toAdd.FechaConsumo = r.GetDateTime(r.GetOrdinal("fecha_consumo")).ToString("yyyy-MM-dd");
				toAdd.ReseCodi = r["rese_codi"].ToString();
				toAdd.TipoCta = r["tipo_cta"].ToString();
				toAdd.Seccion = r["secc_desc"].ToString();
				toAdd.Servicio = r["servicio"].ToString();
				toAdd.Moneda = r["mon"].ToString();
				if (!r.IsDBNull(r.GetOrdinal("importe")))
					toAdd.Importe = (decimal)r["importe"];
				if (!r.IsDBNull(r.GetOrdinal("importe_sol")))
					toAdd.ImporteSol = (decimal)r["importe_sol"];
				toAdd.Ticket = r["nro_ticket"].ToString();
				toAdd.PtoVta = r["pto_vta"].ToString();
				result.Add(toAdd);
			});
			return result;
		}

		public async Task<List<IntakeDetail>> IntakeDetail(string hotel, string ticketNo, string cashier, string year)
		{
			List<IntakeDetail> result = new List<IntakeDetail>();
			await Query(PLSQL.IntakeDetailQuery.Value(hotel, ticketNo, cashier, year), null, r =>
			{
				var toAdd = new IntakeDetail();
				toAdd.NroTicket = r["nro_ticket"].ToString();
				toAdd.Producto = r["producto"].ToString();
				toAdd.GroupDesc = r["grup_desc"].ToString();
				toAdd.FamiDesc = r["fami_desc"].ToString();
			
				if (!r.IsDBNull(r.GetOrdinal("venta")))
					toAdd.Venta = (decimal)r["venta"];
				if (!r.IsDBNull(r.GetOrdinal("dscto")))
					toAdd.Dscto = (decimal)r["dscto"];
				if (!r.IsDBNull(r.GetOrdinal("recargo")))
					toAdd.Recargo = (decimal)r["recargo"];

				result.Add(toAdd);
			});
			return result;
		}

		public object GetService(Type serviceType)
		{
			throw new NotImplementedException();
		}
	}
}
