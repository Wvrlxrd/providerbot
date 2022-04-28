namespace TelegramBot.Database.Models
{
    public class Product
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long Price { get; set; }
        public string Description { get; set; }
        public string ServiceTitle { get; set; }

        public Product(long id, string title, long price, string description, string serviceTitle)
        {
            Id = id;
            Title = title;
            Price = price;
            Description = description;
            ServiceTitle = serviceTitle;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Title)}: {Title}, {nameof(Price)}: {Price}, {nameof(Description)}: {Description}, {nameof(Service)}: {ServiceTitle}";
        }
        
        public string prettyPrint()
        {
            return $"Название: {Title}, Цена: {Price}, Описание: {Description}, относится к услуге: {ServiceTitle}";
        }
    }
}