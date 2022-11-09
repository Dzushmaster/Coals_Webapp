namespace Coals_WebApp.Models.DTO
{
    public class CommentDto
    {
        public uint IdArticle { get; set; }
        public uint IdUser { get; set; }
        public string TextComment { get; set; }
        public DateTime DatetimePublish { get; set; }
    }
}
