using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Command.Commands
{
    public class SearchCommand : Command
    {
        
        public override string[] Names { get; set; } = { "/search"};
        public override string Name { get; set; } = "Поиск";
        public override async void Execute(Message message, TelegramBotClient client)
        {

        }
    }
}