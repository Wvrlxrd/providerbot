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
            await client.SendTextMessageAsync(message.Chat, "Ознакомиться с асортиментом товаров - /products\n" +
                                                            "Ознакомиться с категориями - /category\n" +
                                                            "Ознакомиться с услугами - /service\n" +
                                                            "Найти желаемый товар - /search\n" +
                                                            "Найти по цене - /price");
        }
    }
}
