using Coals_WebApp.Models;
using Coals_WebApp.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coals_WebApp.Controllers
{
    //All of this can be used by authorized user, moderator and admin
    [Route("Comments")]
    public class CommentsController : Controller
    {
        [HttpGet]
        public IActionResult GetComments([FromQuery]int idArticle = 0, int n = 0, int offset = 0)
        {
            if (idArticle <= 0 || n < 0 || offset < 0)
                return new NotFoundResult();
            var result = Comments.GetAllComments(idArticle);
            return Json(result);
        }
        [HttpPost]
        [Route("add")]
        [Authorize(Roles = $"{Roles.Authorized}, {Roles.Moderator}")]
        public IActionResult AddComment([FromBody] CommentDto comment)
        {
            if (comment == null)
                return new ContentResult { StatusCode = 400, Content = "Comment is empty"};
            if (Comments.CheckIsEmpty(comment))
                return new ContentResult { StatusCode = 400, Content = "Not all fields are filled" };
            uint id = 0;
            if ((id = Comments.AddComment(comment, out string error)) == 0)
                return new ContentResult { StatusCode = 400, Content = error };
            return Json(id);
        }
        [HttpDelete]
        [Authorize(Roles = $"{Roles.Authorized}, {Roles.Moderator}")]
        [Route("Remove")]
        public IActionResult RemoveOwnArticle([FromQuery] int id = -1, int idUser = -1)
        {
            if (id == -1 || idUser == -1)
                return new ContentResult { StatusCode = 400, Content = "Not all fields are filled" };
            if (!Comments.RemoveByUser(id, idUser))
                return new ContentResult { StatusCode = 404, Content = "Can't remove this comment" };
            return new OkResult();
        }
        [HttpDelete]
        [Authorize(Roles = Roles.Moderator)]
        [Route("Remove/Moder")]
        public IActionResult RemoveByModerator([FromQuery] int id)
        {
            if (id == -1)
                return new ContentResult { StatusCode = 400, Content = "Not all fields are filled" };
            if (!Comments.RemoveByModer(id))
                return new ContentResult { StatusCode = 404, Content = "Can't remove this comment" };
            return new OkResult();
        }
    }
}
