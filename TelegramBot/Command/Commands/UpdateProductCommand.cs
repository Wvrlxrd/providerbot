using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Command.Commands
{
    public class UpdateProductCommand : Command
    {
        public override string Name { get; set; } = "Обновить продукт";
        public override string[] Names { get; set; } = { "/updateProduct" };
        public override void Execute(Message message, TelegramBotClient client)
        {
            
        }
    }
}