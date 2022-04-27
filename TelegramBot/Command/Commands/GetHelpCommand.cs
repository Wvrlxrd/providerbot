using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Command.Commands
{
    class GetHelpCommand : Command
    {
        public override string[] Names { get; set; } = new string[] { "/help", "помощь" };
        public override string Name { get; set; } = "/help";

        public override async void Execute(Message message, TelegramBotClient client)
        {
            await client.SendTextMessageAsync(message.Chat, "Хелпа"); 
        }
    }
}
