using Coals_WebApp.Models;
using Coals_WebApp.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coals_WebApp.Controllers
{
    [Route("Users")]
    public class UsersController : Controller
    {
        [HttpPost]
        [Route("Register")]
        public IActionResult RegisterUserController([FromBody]RegisterDto candidate)
        {
            if (Users.CheckIsEmpty(candidate))
                return Content("Not all fields are filled");
            if (!Users.RegisterUser(candidate))
                return Content("Invalid values of email or nickname");
            return Ok();
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult LoginUserController([FromBody] LoginDto candidate)
        {
            if (Users.CheckIsEmpty(candidate))
                return Content("Not all fields are filled");
            var result = Users.LoginUser(candidate);
            if (result[0] == Roles.Unauthorized)
                return Unauthorized("Invalid email or password");
            if (result[0] == Roles.Blocked)
                return Unauthorized("You're blocked, try again later");//перенести проверки в users и возвращать просто json с содержимым
            return Json(new {
                access_token = result[0],
                username = result[1]
            });
        }
        [HttpGet]
        [Authorize(Roles = $"{Roles.Authorized}, {Roles.Moderator}")]
        [Route("Logout")]
        public IActionResult LogoutUserController([FromQuery] int id = -1)
        {
            if (id == -1)
                return Content("Can't logout by user with this id");
            if (!Users.Logout(id))
                return Content("Can't logout");
            return Ok();
        }
        [HttpDelete]
        [Authorize(Roles = Roles.Moderator)]
        [Route("Moder/Block")]
        public IActionResult BlockUserController([FromQuery] int userId = -1, int moderatorId = -1)
        {
            if (userId == -1 || moderatorId == -1)
                return Content("Not all fields are filled");
            if (userId == moderatorId)
                return Content("Can't block yourself");
            if (!Users.BlockUser(userId, moderatorId))
                return Content("Cant' block this user");
            return new EmptyResult();
        }
        [HttpGet]
        [Authorize(Roles = Roles.Moderator)]
        [Route("Moder/Unblock")]
        public IActionResult UnblockUserController([FromQuery] int userId = -1, int moderatorId = -1)
        {
            if (userId == -1 || moderatorId == -1)
                return Content("Not all fields are filled");
            if (userId == moderatorId)
                return Content("Can't unblock yourself");
            if (!Users.UnblockUser(userId, moderatorId))
                return Content("Cant' unblock this user");
            return new EmptyResult();
        }
    }
}
