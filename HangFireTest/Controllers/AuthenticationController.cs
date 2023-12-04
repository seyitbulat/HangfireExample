using Microsoft.AspNetCore.Mvc;

namespace HangFireTest.Controllers
{
	public class AuthenticationController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
