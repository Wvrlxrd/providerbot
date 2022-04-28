using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Command.Commands
{
    public class ServiceCommand : Command
    {
        
        public override string[] Names { get; set; } = { "/service"};
        public override string Name { get; set; } = "Услуги";
        public override async void Execute(Message message, TelegramBotClient client)
        {
            var services = Database.Database.GetService();
            foreach (var service in services)
            {
                await client.SendTextMessageAsync(message.Chat, service.prettyPrint());
            }

        }
    }
}