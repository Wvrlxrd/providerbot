﻿using System;
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
        public abstract string Name { get; set; }
        public abstract string [] Names { get; set; }

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
