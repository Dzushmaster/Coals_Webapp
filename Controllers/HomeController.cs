using Coals_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Coals_WebApp.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

    }
}