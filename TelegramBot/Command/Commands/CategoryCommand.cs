using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.Database.Models;

namespace TelegramBot.Command.Commands
{
    public class CategoryCommand : Command
    {
        
        public override string[] Names { get; set; } = { "/category"};
        public override string Name { get; set; } = "Категории";
        public override async void Execute(Message message, TelegramBotClient client)
        {
            var categories = Database.Database.GetCategory();
            foreach (var category in categories)
            {
                await client.SendTextMessageAsync(message.Chat, category.prettyPrint());
            }

        }
    }
}