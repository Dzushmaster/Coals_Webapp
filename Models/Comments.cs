using Coals_WebApp.Models.DTO;

namespace Coals_WebApp.Models
{
    public class Comments
    {
        public uint Id { get; set; }
        public uint IdArticle { get; set; }
        public uint IdUser { get; set; }
        public string? TextComment { get; set; }
        public DateTime DatetimePublish { get; set; }
        //TODO: Add check idArticle
        public static Comments[] GetNComments(int idArticle, int n, int offset) => 
            AppDbContext.GetInstance().comments.Where(comm => comm.IdArticle == idArticle).
            Skip(offset).Take(n).ToArray();
        public static bool CheckIsEmpty(CommentDto dto) =>
            dto.IdArticle == 0 ||
            dto.IdUser == 0 ||
            dto.TextComment == String.Empty ||
            dto.DatePublish == String.Empty;
        //TODO: change true false to return error
        public static bool AddComment(CommentDto dto)
        {
            if (!DateTime.TryParse(dto.DatePublish, out DateTime date))
                return false;
            var dbContext = AppDbContext.GetInstance();
            if (!dbContext.users.Any(user => user.Id == dto.IdUser &&
                (user.Role == Roles.Authorized || user.Role == Roles.Moderator)))
                return false;
            if (!dbContext.articles.Any(article => article.Id == dto.IdArticle))
                return false;
            dbContext.Add(new Comments
            {
                IdArticle = dto.IdArticle,
                IdUser = dto.IdUser,
                TextComment = dto.TextComment,
                DatetimePublish = date
            });
            dbContext.SaveChanges();
            return true;
        }
        //TODO: change true false to return error
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
        //TODO: change true false to return error
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
