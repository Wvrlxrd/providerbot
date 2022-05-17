using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Command.Commands
{
    public class RemoveProductCommand : Command
    {
        public override string Name { get; set; } = "Удалить продукт";
        public override string[] Names { get; set; } = {"/removeProduct"};
        public override void Execute(Message message, TelegramBotClient client)
        {
            
        }
    }
}