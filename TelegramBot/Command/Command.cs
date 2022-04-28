using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Command
{
    public abstract class Command
    {
        // название комманды
        public abstract string Name { get; set; }
        // Триггеры для комманды
        public abstract string [] Names { get; set; }
        // Метод запуска команды
        public abstract void Execute(Message message, TelegramBotClient client);
        
        public bool Contains (string message)
        {
            foreach (var mess in Names)
            {
                if (message.Contains(mess))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
