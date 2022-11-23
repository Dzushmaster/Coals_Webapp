using Coals_WebApp.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Coals_WebApp.Models
{
    public class Users
    {
        public uint Id { get; set; }
        public string? Nickname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        //TODO: change true false to return error
        private static bool FindUserToRegister(RegisterDto candidate)
        {
            return AppDbContext.GetInstance().users.FirstOrDefault(user =>
            candidate.Nickname == user.Nickname ||
            candidate.Email == user.Email) != null;
        }
        public static bool CheckIsEmpty(RegisterDto candidate)
        {
            return candidate.Nickname == string.Empty ||
                candidate.Email == string.Empty ||
                candidate.Password == string.Empty;
        }
        public static bool CheckIsEmpty(LoginDto candidate)
        {
            return candidate.Email == string.Empty ||
                candidate.Password == string.Empty;
        }
        //TODO: change true false to return error
        public static bool RegisterUser(RegisterDto newUser)
        {
            if (FindUserToRegister(newUser))
                return false;
            var instanse = AppDbContext.GetInstance();
            instanse.users.Add(new Users
            {
                Nickname = newUser.Nickname,
                Email = newUser.Email,
                Password = newUser.Password,
                Role = Roles.Authorized
            });
            instanse.SaveChangesAsync();
            return true;
        }
        //TODO: change true false to return error
        public static string[] LoginUser(LoginDto loginUser)
        {
            AppDbContext dbContext = AppDbContext.GetInstance();
            var user = dbContext.users.FirstOrDefault(user => loginUser.Email == user.Email && user.Password == loginUser.Password);
            if (user == null)
                return new string[] {Roles.Unauthorized};
            else if (user.Role == Roles.Blocked)
                return new string[] { Roles.Blocked };
            return GenerateToken(user.Nickname, user.Role);
        }
        //TODO: remove change role to unauthorized
        //TODO: change true false to return error
        public static bool Logout(int id)
        {
            AppDbContext dbContext = AppDbContext.GetInstance();
            var user = dbContext.users.FirstOrDefault(user => user.Id == id);
            if (user == null || user.Role == Roles.Unauthorized || user.Role == Roles.Blocked)
                return false;
            user.Role = Roles.Unauthorized;
            dbContext.SaveChangesAsync();
            return true;
        }
        //TODO: change true false to return error
        public static bool BlockUser(int userId, int moderatorId)
        {
            AppDbContext dbContext = AppDbContext.GetInstance();
            var user = dbContext.users.FirstOrDefault(user => user.Id == userId);
            var moderator = dbContext.users.FirstOrDefault(user => user.Id == moderatorId);
            if (user == null || moderator == null || user.Role != Roles.Authorized || moderator.Role != Roles.Moderator)
                return false;
            user.Role = Roles.Blocked;
            dbContext.SaveChangesAsync();
            return true;
        }
        //TODO: change true false to return error
        public static bool UnblockUser(int userId, int moderatorId)
        {
            AppDbContext dbContext = AppDbContext.GetInstance();
            var Users = dbContext.users.AsQueryable();
            var user = Users.FirstOrDefault(user => user.Id == userId);
            var moderator = Users.FirstOrDefault(user => user.Id == moderatorId);
            if (user == null || moderator == null || user.Role != Roles.Blocked|| moderator.Role != Roles.Moderator)
                return false;
            user.Role = Roles.Authorized;
            dbContext.SaveChangesAsync();
            return true;
        }
        private static string[] GenerateToken(string username, string role)
        {
            var claim = GetIdentity(username, role);
            var jwt = new JwtSecurityToken(
                issuer: Program.AuthOptions.ISSUER,
                audience: Program.AuthOptions.AUDIENCE,
                claims: claim.Claims,
                expires: DateTime.UtcNow.AddDays(10),
                signingCredentials: new SigningCredentials(Program.AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                );
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new string[] {token, claim.Name};
        }
        private static ClaimsIdentity GetIdentity(string username, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, username),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultNameClaimType);
            return claimsIdentity;
        }
    }
}
