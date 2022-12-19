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
        public IActionResult GetArticles([FromQuery]int n = -1, int offset = -1)
        {
            if (n < 0 || offset < 0)
                return new ContentResult { StatusCode = 400, Content = "Not all fields are filled" };
            var result = Articles.GetAllArticles();
            if(result.Length == 0)
                return new ContentResult { StatusCode = 404, Content = "Can't find any article" };
            return Json(result);
        }
        [Route("find")]
        [HttpGet]
        public IActionResult GetArticleByName([FromQuery] string name = "")
        {
            if (name == String.Empty)
                return new ContentResult { StatusCode = 400, Content = "Not all fields are filled" };
            var result = Articles.FindArticleByName(name);
            if(result == null || result.Length == 0)
                return new ContentResult { StatusCode = 404, Content = "Cant find article with this name" };
            return Json(result);
        }
        //[Route("id")]
        //[HttpGet]
        //public IActionResult GetArticleById([FromQuery] int id)
        //{
        //    if(id < 0)
        //        return new ContentResult { StatusCode = 400, Content = "Article is not found" };
        //    return 
        //}
        [HttpPost]
        [Route("add")]
        [Authorize(Roles = $"{Roles.Authorized}, {Roles.Moderator}")]
        public IActionResult AddArticle([FromBody] ArticleDto article)
        {
            if (article == null)
                return new ContentResult { StatusCode = 400, Content = "Article is empty" };
            if (Articles.CheckIsEmpty(article))
                return new ContentResult { StatusCode = 400, Content = "Not all fields are filled" };
            if (!Articles.AddArticle(article, out string error))
                return new ContentResult { StatusCode = 400, Content = error };
            return new OkResult();
        }
        [HttpDelete]
        [Authorize(Roles = $"{Roles.Authorized}, {Roles.Moderator}")]
        [Route("Remove")]
        public IActionResult RemoveOwnArticle([FromQuery] int id = -1, int idUser = -1)
        {
            if(id == -1 || idUser == -1)
                return new ContentResult { StatusCode = 400, Content = "Not all fields are filled" };
            if (!Articles.RemoveByUser(id, idUser))
                return new ContentResult { StatusCode = 404, Content = "Can't remove this article" };
            return new OkResult();
        }
        [HttpDelete]
        [Authorize(Roles = Roles.Moderator)]
        [Route("Remove/Moder")]
        public IActionResult RemoveByModerator([FromQuery] int id)
        {
            if (id == -1)
                return new ContentResult { StatusCode = 400, Content = "Not all fields are filled" };
            if (!Articles.RemoveByModer(id))
                return new ContentResult { StatusCode = 404, Content = "Can't remove this article" };
            return new OkResult();
        }
    }
}
