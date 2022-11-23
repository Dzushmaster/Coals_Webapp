﻿using Coals_WebApp.Models;
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
            if (idArticle <= 0 || n <= 0 || offset < 0)
                return NotFound();
            var result = Comments.GetNComments(idArticle, n, offset);
            if (result.Length == 0)
                return NotFound();
            return Json(result);
        }
        [HttpPost]
        [Authorize(Roles = Roles.Authorized)]
        public IActionResult AddComment([FromBody] CommentDto comment)
        {
            if (comment == null)
                return NotFound();
            if (Comments.CheckIsEmpty(comment))
                return NotFound();
            if (!Comments.AddComment(comment))
                return NotFound("Invalid comment");
            return new EmptyResult();
        }
        [HttpDelete]
        [Authorize(Roles = $"{Roles.Authorized}, {Roles.Moderator}")]
        [Route("Remove")]
        public IActionResult RemoveOwnArticle([FromQuery] int id = -1, int idUser = -1)
        {
            if (id == -1 || idUser == -1)
                return NotFound();
            if (!Comments.RemoveByUser(id, idUser))
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
            if (!Comments.RemoveByModer(id))
                return NotFound();
            return new EmptyResult();
        }

    }
}
