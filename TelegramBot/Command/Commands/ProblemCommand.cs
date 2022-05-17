using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Command.Commands
{
    public class ProblemCommand : Command
    {
        public override string Name { get; set; } = "Контак при проблемах";
        public override string[] Names { get; set; } = { "/problem" };
        public override void Execute(Message message, TelegramBotClient client)
        {
            throw new System.NotImplementedException();
        }
    }
}