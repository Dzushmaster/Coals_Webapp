using Coals_WebApp.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Coals_WebApp.Models
{
    public class Users
    {
        public uint Id { get; set; }
        public string? Nickname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public Roles Role { get; set; }
        private static bool FindUserToRegister(RegisterDto candidate)
        {
            return AppDbContext.GetInstance().users.FirstOrDefault(user =>
            candidate.Nickname == user.Nickname ||
            candidate.Email == user.Email) != null;
        }
        public static bool CheckEmpty(RegisterDto candidate)
        {
            return candidate.Nickname == string.Empty ||
                candidate.Email == string.Empty ||
                candidate.Password == string.Empty;
        }
        public static bool CheckEmpty(LoginDto candidate)
        {
            return candidate.Email == string.Empty ||
                candidate.Password == string.Empty;
        }
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
        public static Roles LoginUser(LoginDto loginUser)
        {
            AppDbContext dbContext = AppDbContext.GetInstance();
            Users user = dbContext.users.FirstOrDefault(user => loginUser.Email == user.Email && user.Password == loginUser.Password);
            if (user == null)
                return Roles.Unauthorized;
            return user.Role;
        }
        //remove change role to unauthorized
        public static bool Logout(int id)
        {
            AppDbContext dbContext = AppDbContext.GetInstance();
            Users user = dbContext.users.FirstOrDefault(user => user.Id == id);
            if (user == null || user.Role == Roles.Unauthorized || user.Role == Roles.Blocked)
                return false;
            user.Role = Roles.Unauthorized;
            dbContext.SaveChangesAsync();
            return true;
        }
        public static bool BlockUser(int userId, int moderatorId)
        {
            AppDbContext dbContext = AppDbContext.GetInstance();
            Users user = dbContext.users.FirstOrDefault(user => user.Id == userId);
            Users moderator = dbContext.users.FirstOrDefault(user => user.Id == moderatorId);
            if (user == null || moderator == null || user.Role != Roles.Authorized || moderator.Role != Roles.Moderator)
                return false;
            user.Role = Roles.Blocked;
            dbContext.SaveChangesAsync();
            return true;
        }
        public static bool UnblockUser(int userId, int moderatorId)
        {
            AppDbContext dbContext = AppDbContext.GetInstance();
            var Users = dbContext.users.AsQueryable();
            Users user = Users.FirstOrDefault(user => user.Id == userId);
            Users moderator = Users.FirstOrDefault(user => user.Id == moderatorId);
            if (user == null || moderator == null || user.Role != Roles.Blocked|| moderator.Role != Roles.Moderator)
                return false;
            user.Role = Roles.Authorized;
            dbContext.SaveChangesAsync();
            return true;
        }
    }
}
