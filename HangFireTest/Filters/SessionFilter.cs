using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HangFireTest.Filters
{
	public class SessionFilter : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var sessionJson = context.HttpContext.Session.GetString("HangFireAdmin");
			if (string.IsNullOrEmpty(sessionJson))
			{
				context.Result = new RedirectResult("~/Authentication/LogIn");
			}
		}
	}
}
