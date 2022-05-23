using System;
using System.Collections.Generic;

namespace TelegramBot.Database.Models
{
    public class Category
    {
        public long Id { get; }
        public string Title { get; }

        public Category(long id, string title)
        {
            Id = id;
            Title = title;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Title)}: {Title}";
        }

        public string prettyPrint()
        {
            return $"Название: {Title}";
        }
    }
    
}