using Hangfire.Dashboard;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace HangFireTest.Models
{
	public class AuthFilter : IDashboardAuthorizationFilter
	{

		public AuthFilter()
		{
			
		}

		public bool Authorize(DashboardContext context)
		{
			var httpContext = context.GetHttpContext();

			var sesTest =  httpContext.Session.GetString("hangfireAdmin");

			if(string.IsNullOrEmpty(sesTest))
				return false;


			return true;
		}
	}
}
