using HangFireTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace HangFireTest.Controllers
{
	public class AuthenticationController : Controller
	{
        [HttpGet]
		public IActionResult Index()
		{
			return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginDto dto)
        {
            if (dto.UserName == "Admin" && dto.Password == "123456")
            {
                string serializedData = JsonSerializer.Serialize(true);
                byte[] byteArray = Encoding.UTF8.GetBytes(serializedData);
                HttpContext.Session.Set("hangfireAdmin", byteArray);
                return Json(new { IsSuccess = true });
            }
            else
            {
                return Unauthorized(); 
            }
        }



    }
}
