using Coals_WebApp.Models.DTO;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Coals_WebApp.Models
{
    public class Comments
    {
        public uint Id { get; set; }
        public uint IdArticle { get; set; }
        public uint IdUser { get; set; }
        public string? TextComment { get; set; }
        public DateTime DatetimePublish { get; set; }
        public static Comments[] GetAllComments(int idArticle) => 
            AppDbContext.GetInstance().comments.Where(comm => comm.IdArticle == idArticle).ToArray();
        public static Comments[] GetNComments(int idArticle, int n, int offset) => 
            AppDbContext.GetInstance().comments.Where(comm => comm.IdArticle == idArticle).
            Skip(offset).Take(n).ToArray();
        public static bool CheckIsEmpty(CommentDto dto) =>
            dto.IdArticle == 0 ||
            dto.IdUser == 0 ||
            dto.TextComment == String.Empty ||
            dto.DatePublish == String.Empty;
        public static uint AddComment(CommentDto dto, out string error)
        {
            error = "";
            if (!DateTime.TryParse(dto.DatePublish, out DateTime date))
            {
                error = "Invalid date format";
                return UInt32.MinValue;
            }
            var dbContext = AppDbContext.GetInstance();
            if (!dbContext.users.Any(user => user.Id == dto.IdUser &&
                (user.Role == Roles.Authorized || user.Role == Roles.Moderator)))
            {
                error = "This user can't comment this article";
                return UInt32.MinValue;
            }
            if (!dbContext.articles.Any(article => article.Id == dto.IdArticle))
            {
                error = "Can't find this article";
                return UInt32.MinValue;
            }
            dbContext.Add(new Comments
            {
                IdArticle = dto.IdArticle,
                IdUser = dto.IdUser,
                TextComment = dto.TextComment,
                DatetimePublish = date
            });
            dbContext.SaveChanges();
            var id = dbContext.comments.FirstOrDefault(
                comm => comm.IdArticle == dto.IdArticle &&
                comm.IdUser == dto.IdUser &&
                comm.TextComment == dto.TextComment).Id;

            return id;
        }
        public static bool RemoveByUser(int id, int idUser)
        {
            var dbContext = AppDbContext.GetInstance();
            var comments = dbContext.comments;
            var delete = comments.FirstOrDefault(comment => comment.Id == id && comment.IdUser == idUser);
            if (delete == null)
                return false;
            comments.Remove(delete);
            dbContext.SaveChanges();
            return true;
        }
        public static bool RemoveByModer(int id)
        {
            var dbContext = AppDbContext.GetInstance();
            var comments = dbContext.comments;
            var delete = comments.FirstOrDefault(comment => comment.Id == id);
            if (delete == null)
                return false;
            comments.Remove(delete);
            dbContext.SaveChanges();
            return true;
        }

    }
}
