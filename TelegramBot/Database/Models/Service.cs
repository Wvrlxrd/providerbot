namespace TelegramBot.Database.Models
{
    public class Service
    {
        public long Id { get; set; }
        public string Title { get; set; }
        
        public string CategoryTitle { get; set; }

        public Service(long id, string title, string categoryTitle)
        {
            Id = id;
            Title = title;
            CategoryTitle = categoryTitle;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Title)}: {Title}, {nameof(CategoryTitle)}: {CategoryTitle}";
        }
    }
}