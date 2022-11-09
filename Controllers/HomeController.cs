using Coals_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Coals_WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Ok();
        }

    }
}