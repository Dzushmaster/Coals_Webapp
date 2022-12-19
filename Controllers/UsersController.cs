using Coals_WebApp.Models;
using Coals_WebApp.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Coals_WebApp.Controllers
{
    [Route("Users")]
    public class UsersController : Controller
    {
        [HttpPost]
        [Route("Register")]
        public IActionResult RegisterUser([FromBody]RegisterDto candidate)
        {
            if (Users.CheckIsEmpty(candidate))
                return new ContentResult { StatusCode = 400, Content = "Not all fields are filled" };
            if (!Users.RegisterUser(candidate))
                return new ContentResult { StatusCode = 400, Content = "Invalid values of email or nickname" };
            return new OkResult();
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult LoginUser([FromBody] LoginDto candidate)
        {
            if (Users.CheckIsEmpty(candidate))
                return new ContentResult { StatusCode = 400, Content = "Not all fields are filled" };
            var result = Users.LoginUser(candidate);
            if (result[0] == Roles.Unauthorized)
                return new ContentResult { StatusCode = 401, Content = "Invalid email or password" };
            if (result[0] == Roles.Blocked)
                return new ContentResult { StatusCode = 401, Content = "You're blocked, try again later" };//перенести проверки в users и возвращать просто json с содержимым
            return Json(new {
                access_token = result[0]
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
        public IActionResult BlockUser([FromQuery] int userId = -1)
        {
            if (userId == -1)
                return new ContentResult { StatusCode = 400, Content = "Not all fields are filled" };
            if (!Users.BlockUser(userId))
                return new ContentResult { StatusCode = 400, Content = "Cant' block this user" };
            return new OkResult();
        }
        [HttpGet]
        [Authorize(Roles = Roles.Moderator)]
        [Route("Moder/Unblock")]
        public IActionResult UnblockUser([FromQuery] int userId = -1)
        {
            if (userId == -1)
                return new ContentResult { StatusCode = 400, Content = "Not all fields are filled" };
            if (!Users.UnblockUser(userId))
                return new ContentResult { StatusCode = 400, Content = "Cant' unblock this user" };
            return new OkResult();
        }
        [HttpGet]
        [Authorize(Roles=Roles.Moderator)]
        [Route("Moder/Blocked")]
        public IActionResult GetBlockedUsers()
        {
            var blocked = Users.BlockedUsers();
            return Json(blocked);
        }
    }
}
