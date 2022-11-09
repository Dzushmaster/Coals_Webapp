namespace Coals_WebApp.Models
{
    public class Comments
    {
        public uint Id { get; set; }
        public uint IdArticle { get; set; }
        public uint IdUser { get; set; }
        public string TextComment { get; set; }
        public DateTime DatetimePublish { get; set; }
    }
}
