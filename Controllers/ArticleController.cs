using Coals_WebApp.Models;
using Coals_WebApp.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coals_WebApp.Controllers
{
    [Route("Articles")]
    public class ArticleController : Controller
    {
        [HttpGet]
        public IActionResult GetArticles([FromQuery]int n = 0, int offset = 0)
        {
            if (n <= 0 || offset < 0)
                return NotFound();
            return Json(Articles.GetNArticles(n, offset));
        }
        [Route("find")]
        [HttpGet]
        public IActionResult GetArticleByName([FromQuery] string name = "")
        {
            if (name == "")
                return Content("Write name to search");
            var result = Articles.FindArticleByName(name);
            if(result == null || result.Length == 0)
                return Content("Cant find article with this name");
            return Json(result);
        }
        [HttpPost]
        [Authorize(Roles = Roles.Authorized)]
        public IActionResult AddArticle([FromBody] ArticleDto article)
        {
            if (article == null)
                return NotFound();
            if(Articles.CheckIsEmpty(article))
                return NotFound();
            if (!Articles.AddArticle(article))
                return NotFound("Invalid article");
            return new EmptyResult();
        }
        [HttpDelete]
        [Authorize(Roles = $"{Roles.Authorized}, {Roles.Moderator}")]
        [Route("Remove")]
        public IActionResult RemoveOwnArticle([FromQuery] int id = -1, int idUser = -1)
        {
            if(id == -1 || idUser == -1)
                return NotFound();
            if (!Articles.RemoveByUser(id, idUser))
                return NotFound();
            return new EmptyResult();
        }
        [HttpDelete]
        [Authorize(Roles = Roles.Moderator)]
        [Route("Remove/Moder")]
        public IActionResult RemoveByModerator([FromQuery] int id)
        {
            if (id == -1)
                return NotFound();
            if (!Articles.RemoveByModer(id))
                return NotFound();
            return new EmptyResult();
        }
    }
}
