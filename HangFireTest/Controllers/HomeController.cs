using Hangfire;
using HangFireTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TestService;

namespace HangFireTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CurrencyService _service;
        private readonly CurrencyTrigger _trigger;

        public HomeController(ILogger<HomeController> logger, CurrencyService service, CurrencyTrigger trigger)
        {
            _logger = logger;
            _service = service;
            _trigger = trigger;
        }

        public async Task<IActionResult> Index()
        {

            var list = await _service.GetCurrencies();
            var message = new SocketMessage<List<Currency>>()
            {
                Message = list,
                Time = DateTime.Now.ToShortTimeString()
            };
            RecurringJob.AddOrUpdate("GetCurrencies", () => _trigger.GetCurrencies() , "* * * * *",TimeZoneInfo.Local);
            return View(message);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
