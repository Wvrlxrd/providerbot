using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Command.Commands
{
    public class PriceCommand : Command
    {
        
        public override string[] Names { get; set; } = { "/price"};
        public override string Name { get; set; } = "В ценновом диапозоне";
        public override async void Execute(Message message, TelegramBotClient client)
        {
        }
    }
}