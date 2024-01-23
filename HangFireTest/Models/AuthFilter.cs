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

			var adminTest =  httpContext.Session.GetString("hangfireAdmin");

			if (string.IsNullOrEmpty(adminTest))
			{
				context.Response.ContentType = "text/html";
                context.Response.WriteAsync(GetUnauthorizedHtml());

            }


			return true;
		}

        private string GetUnauthorizedHtml()
        {
            return @"
        <!DOCTYPE html>
        <html lang=""tr"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>Yetkisiz Erişim</title>
            <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css"" rel=""stylesheet"">
        </head>
        <body class=""bg-light"">
            <div class=""container mt-5"">
                <div class=""alert alert-danger"" role=""alert"">
                    <h4 class=""alert-heading"">Yetkisiz Erişim</h4>
                    <p>Bu kaynağa erişim izniniz bulunmamaktadır.</p>
                    <hr>
                    <p class=""mb-0"">Yardım için lütfen yöneticiyle iletişime geçin.</p>
                    <div class=""mt-3"">
                        <a href=""https://localhost:44384/authentication"" class=""btn btn-primary"">Giriş Yap</a>
                        
                    </div>
                </div>
            </div>
        </body>
        </html>
    ";
        }

    }
}
