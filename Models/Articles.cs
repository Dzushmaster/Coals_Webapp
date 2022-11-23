using Coals_WebApp.Models.DTO;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;

namespace Coals_WebApp.Models
{
    public class Articles
    {
        public uint Id { get; set; }
        public uint IdUser { get; set; }
        public string? Name { get; set; }
        public string? TextArticle { get; set; }
        public DateTime DatetimePublish { get; set; }
        //TODO: change true false to return error

        public static Articles[] GetNArticles(int count, int offset) => AppDbContext.GetInstance().articles.Skip(offset).Take(count).ToArray();
        //TODO: change true false to return error

        public static Articles[] FindArticleByName(string name) => AppDbContext.GetInstance().articles.Where(n => n.Name.Contains(name)).ToArray();

        public static bool CheckIsEmpty(ArticleDto article) =>
            article.IdUser == 0 ||
            article.Name == String.Empty ||
            article.TextArticle == String.Empty ||
            article.DatePublish == String.Empty;
        //TODO: change true false to return error
        public static bool AddArticle(ArticleDto dto)
        {
            if (!DateTime.TryParse(dto.DatePublish, out DateTime date))
                return false;
            var dbContext = AppDbContext.GetInstance();
            if (!dbContext.users.Any(user => user.Id == dto.IdUser && 
                (user.Role == Roles.Authorized || user.Role == Roles.Moderator)))
                return false;
            if (dbContext.articles.Any(article => dto.Name == article.Name))
                return false;
            dbContext.Add(new Articles
            {
                IdUser = dto.IdUser,
                Name = dto.Name,
                TextArticle = dto.TextArticle,
                DatetimePublish = date
            });
            dbContext.SaveChanges();
            return true;
        }
        //TODO: change true false to return error
        public static bool RemoveByUser(int id, int idUser)
        {
            var dbContext = AppDbContext.GetInstance();
            var articles = dbContext.articles;
            var delete = articles.FirstOrDefault(article => article.Id == id && article.IdUser == idUser);
            if (delete == null)
                return false;
            articles.Remove(delete);
            dbContext.SaveChanges();
            return true;
        }
        //TODO: change true false to return error
        public static bool RemoveByModer(int id)
        {
            var dbContext = AppDbContext.GetInstance();
            var articles = dbContext.articles;
            var delete = articles.FirstOrDefault(article => article.Id == id);
            if (delete == null)
                return false;
            articles.Remove(delete);
            dbContext.SaveChanges();
            return true;
        }
    }
}
