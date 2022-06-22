using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace netapi
{
	public class MyAuth : Attribute, IAuthorizationFilter
	{
		public string[] Roles { get; set; }
		public MyAuth(params string[] roles)
		{
			this.Roles = roles;
		}
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			var http = context.HttpContext;
			try
			{
				string token = http.Request.Headers["token"];
				if (token == null)
				{
					throw new MyException("Token inválido.");
				}

				TokenBE tokenBE = JsonConvert.DeserializeObject<TokenBE>(MyCrypt.Decrypt(token));
				if (tokenBE.Expiration < DateTime.Now)
				{
					throw new MyException("Token expirado.");
				}

				if (Roles.Length > 0)
				{
					if (tokenBE.IsAdmin && !Roles.Contains("admin"))
					{
						throw new MyException("No autorizado para realizar esta operación.");
					}

					if (!tokenBE.IsAdmin && !Roles.Contains("client"))
					{
						throw new MyException("No autorizado para realizar esta operación.");
					}
				}
			}
			catch (MyException ex)
			{
				context.Result = new UnauthorizedObjectResult(new ResponseBE() { status = false, message = ex.Message });
			}
			catch (Exception ex)
			{
				context.Result = new UnauthorizedObjectResult(new ResponseBE() { status = false, message = ex.Message });
			}
		}
	}
}
