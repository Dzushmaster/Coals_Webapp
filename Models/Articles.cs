namespace Coals_WebApp.Models
{
    public class Articles
    {
        public uint Id { get; set; }
        public uint IdUser { get; set; }
        public string Name { get; set; }
        public string TextArticle { get; set; }
        public DateTime DatetimePublish { get; set; }
    }
}
