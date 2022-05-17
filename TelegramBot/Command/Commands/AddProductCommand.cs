using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Command.Commands
{
    public class AddProductCommand : Command
    {
        public override string Name { get; set; } = "Добавить продукт";
        public override string[] Names { get; set; } = { "/addProduct" };
        public override void Execute(Message message, TelegramBotClient client)
        {
            
        }
    }
}