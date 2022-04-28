using System.Runtime.InteropServices;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Command.Commands
{
    public class ProductCommand : Command
    {
        
        public override string[] Names { get; set; } = { "/product", "/products"};
        public override string Name { get; set; } = "Товары";
        public override async void Execute(Message message, TelegramBotClient client)
        {
            var products = Database.Database.GetProduct();
            foreach (var product in products)
            {
                await client.SendTextMessageAsync(message.Chat, product.prettyPrint());
            }

        }
    }
}