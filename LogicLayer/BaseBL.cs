using Common;
using DataLayer;
using Newtonsoft.Json;
using Security;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
	public class BaseBL
	{
		public void ValidateToken(IHasToken hasToken, MyRole myRole)
		{			
			if (hasToken.Token == null)
				throw new TokenException("Token inválido.");
			TokenBE tokenBE;
			try
			{
				tokenBE = JsonConvert.DeserializeObject<TokenBE>(MyCrypt.Decrypt(hasToken.Token));
			}
			catch (Exception)
			{
				throw new TokenException("Hubo un problema al leer su token, ingrese sus credenciales nuevamente por favor.");
			}
			
			if (tokenBE.Expiration < DateTime.Now)
				throw new TokenException("Token expirado.");

			switch (myRole)
			{
				case MyRole.Admin:
					if (!tokenBE.IsAdmin)
						throw new TokenException("No está autorizado para realizar esta operación.");
					break;
				case MyRole.Client:
					if (tokenBE.IsAdmin)
						throw new TokenException("No está autorizado para realizar esta operación.");
					break;
				case MyRole.Both:
					break;
			}

			hasToken.TokenBE = tokenBE;
		}

		public async Task<ResponseBE> GetResponse(IHasToken hasToken, MyRole myRole, Func<ResponseBE, Task> myaction)
		{
			ResponseBE response = new ResponseBE();
			try
			{
				ValidateToken(hasToken, myRole);
				await myaction(response);
				response.status = true;
			}
			catch (TokenException ex)
			{
				response.logout = true;
				response.status = false;
				response.message = ex.Message;
				response.data = null;
			}
			catch (MyException ex)
			{
				response.status = false;
				response.message = ex.Message;
				response.data = null;
			}
			catch (Exception ex)
			{
				response.status = false;
				response.message = ex.Message;
				//response.message = "Ocurrió un error";
				response.data = null;
			}
			return response;
		}

		public async Task<ResponseBE> GetResponseTransaction(IHasToken hasToken, MyRole myRole, MyContext context, Func<ResponseBE, Task> myaction)
		{
			ResponseBE response = new ResponseBE();
			try
			{
				ValidateToken(hasToken, myRole);
				var transaction = await context.Database.BeginTransactionAsync();
				try
				{
					await myaction(response);
					await transaction.CommitAsync();
					response.status = true;
				}
				catch (Exception)
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
			catch (TokenException ex)
			{
				response.logout = true;
				response.status = false;
				response.message = ex.Message;
				response.data = null;
			}
			catch (MyException ex)
			{
				response.status = false;
				response.message = ex.Message;
				response.data = null;
			}
			catch (Exception ex)
			{
				response.status = false;
				response.message = ex.Message;
				//response.message = "Ocurrió un error";
				response.data = null;
			}
			return response;
		}
	}
}
