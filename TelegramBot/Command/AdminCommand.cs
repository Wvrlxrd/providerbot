using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Command
{
    public class AdminCommand : Command
    {
        public override string Name { get; set; } = "Админ панель";
        public override string[] Names { get; set; } = { "/admin" };
        public override void Execute(Message message, TelegramBotClient client)
        {
            
        }
    }
}