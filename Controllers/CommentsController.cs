using Coals_WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Coals_WebApp.Controllers
{
    //All of this can be used by authorized user, moderator and admin
    [Route("Comments")]
    public class CommentsController : Controller
    {
        [HttpGet]
        public IActionResult GetAllComments([FromQuery]int articleId)
        {
            return View();
        }
        //maybe, add check 0 length of TextComment
        [HttpPost]
        public IActionResult AddComment([FromBody] Comments comment)
        {
            return View();
        }
    }
}
