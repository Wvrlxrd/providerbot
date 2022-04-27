using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Command.Commands;

namespace TelegramBot
{
    class Programm
    {
        public static TelegramBotClient botClient;
        private static List<Command.Command> commands;
        static void Main(string[] args)
        {
            botClient = new TelegramBotClient("5193189050:AAEOvghnaxMeUVDAZtz1PrBsn7bTNg2g3wA"); 
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = {},
            };

            commands = new List<Command.Command>(); // Создание объекта класса Command
            commands.Add(new GetHelpCommand());    // Инициализация комманд 

            botClient.StartReceiving(        // Подписка на события
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token);
            Console.WriteLine("Бот начал работу");
            Console.ReadLine();
        }

        private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update?.Message?.Text != null)
            {
                await HandleMessage(botClient, update.Message);
                return;
            }
        }
        
        private static async Task HandleMessage (ITelegramBotClient botClient, Message message)
        {
            if (message.Text == "/start")
            {
  
                await botClient.SendTextMessageAsync(message.Chat.Id, "Выберите команду: " );
                
            }
            foreach (var comm in commands)
            {
                if (comm.Contains(message.Text))        // Проверка на начилие комманды в списке
                {
                    comm.Execute(message, (TelegramBotClient)botClient); // Выполнение комманды
                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Такой комманды не существует.");
                }
            }
        }

        private static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception)); // Вывод ошибок
        }

    }
}
