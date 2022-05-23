using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Command.Commands
{
    public class GetProductByCategoryCommand : Command
    {
        public override string Name { get; set; } = "Получить все продукты по категории";
        public override string[] Names { get; set; } = { "/getProductCategory" };
        public override void Execute(Message message, TelegramBotClient client)
        {
        }
    }
}