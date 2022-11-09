using Coals_WebApp.Models;
using Coals_WebApp.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Coals_WebApp.Controllers
{
    [Route("Users")]
    public class UsersController : Controller
    {
        [HttpPost]
        [Route("Register")]
        public IActionResult RegisterUserController([FromBody]RegisterDto candidate)
        {
            if (Users.CheckEmpty(candidate))
                return Content("Not all fields are filled");
            if (!Users.RegisterUser(candidate))
                return Content("Invalid values of email or nickname");
            return Ok();
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult LoginUserController([FromBody] LoginDto candidate)
        {
            if (Users.CheckEmpty(candidate))
                return Content("Not all fields are filled");
            Roles result = Users.LoginUser(candidate);
            if (result == Roles.Unauthorized)
                return Content("Invalid email or password");
            if (result == Roles.Blocked)
                return Content("You're blocked, try again later");
            return Ok();
        }
        [HttpGet]
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
        [Route("Moder/Block")]
        public IActionResult BlockUserController([FromQuery] int userId = -1, int moderatorId = -1)
        {
            if (userId == -1 || moderatorId == -1)
                return Content("Not all fields are filled");
            if (userId == moderatorId)
                return Content("Can't block yourself");
            if (!Users.BlockUser(userId, moderatorId))
                return Content("Cant' block this user");
            return Ok();
        }
        [HttpGet]
        [Route("Moder/Unblock")]
        public IActionResult UnblockUserController([FromQuery] int userId = -1, int moderatorId = -1)
        {
            if (userId == -1 || moderatorId == -1)
                return Content("Not all fields are filled");
            if (userId == moderatorId)
                return Content("Can't unblock yourself");
            if (!Users.UnblockUser(userId, moderatorId))
                return Content("Cant' unblock this user");
            return Ok();
        }
    }
}
