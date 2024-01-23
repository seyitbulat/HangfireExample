using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using System.Text.Json;

namespace HangFireTest.Filters
{
    public class SessionExpirationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var adminTest = filterContext.HttpContext.Session.GetString("hangfireAdmin");

            // Kullanıcı oturumu varsa ve URL değişikliği algılandıysa
            if (!(string.IsNullOrEmpty(adminTest)) && UrlChanged(filterContext))
            {
                // Oturumu sonlandırabilirsiniz
                filterContext.HttpContext.Session.Clear();
                filterContext.Result = new RedirectResult("/Authentication"); // Yönlendirme yapabilirsiniz
            }

            base.OnActionExecuting(filterContext);
        }

        private bool UrlChanged(ActionExecutingContext filterContext)
        {
            var currentUrl = filterContext.HttpContext.Request.GetDisplayUrl();
            var originalUrl = filterContext.HttpContext.Session.GetString("OriginalUrl");

            // İlk kez giriş yapıldığında veya URL değiştiğinde
            if (string.IsNullOrEmpty(originalUrl) || !string.Equals(originalUrl, currentUrl, StringComparison.OrdinalIgnoreCase))
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(currentUrl);
                // Güncel URL'yi sakla
                filterContext.HttpContext.Session.Set("OriginalUrl", byteArray);
                return true;
            }

            return false;
        }
    }
}
