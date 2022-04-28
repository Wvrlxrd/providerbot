﻿using System;
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
using TelegramBot.Database.Models;

namespace TelegramBot
{
    class Programm
    {
        public static TelegramBotClient botClient;
        private static List<Command.Command> commands;
        private static bool ModeSearch = false;
        private static bool ToMatch = false;
        private static int step = 0;
        private static bool ModePrice = false;
        private static string helloString = "Здравствуйте! Я ваш электронный консультант. В нашем боте вы можете:\n" +
                                            "Ознакомиться с асортиментом товаров - /products\n" +
                                            "Ознакомиться с категориями - /category\n" +
                                            "Ознакомиться с услугами - /service" +
                                            "Найти желаемый товар - /search";
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
            commands.Add(new CategoryCommand());    // Инициализация комманд 
            commands.Add(new SearchCommand());    // Инициализация комманд 
            commands.Add(new ServiceCommand());    // Инициализация комманд 
            commands.Add(new ProductCommand());    // Инициализация комманд 

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
                
                await botClient.SendTextMessageAsync(message.Chat.Id, helloString);
                return;
            }
            if (ModeSearch)
            {
                SearchRun(message, (TelegramBotClient)botClient);
                return;
            }
            foreach (var comm in commands)
            {
                // Проверка на начилие комманды в списке
                if (comm.Contains(message.Text) && !ModeSearch && !ModePrice)
                {
                    if(message.Text == "/search")
                    {
                        ModeSearch = true;
                    }
                    if(message.Text == "/price")
                    {
                        ModePrice = true;
                    }
                    comm.Execute(message, (TelegramBotClient)botClient);
                    ToMatch = true;
                }
            }
            if (ModeSearch)
            {
                SearchRun(message, (TelegramBotClient)botClient);
                return;
            }

            if (ModePrice)
            {
                
            }
            if (!ToMatch && !ModeSearch && !ModePrice)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Такой команды не существует.");
            }
            
            
        }

        private static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception)); // Вывод ошибок
        }

        private static async Task SearchRun(Message message, TelegramBotClient client)
        {
            
            switch (step)
            {
                case 0:
                    await client.SendTextMessageAsync(message.Chat, "Введите запрос");
                    step += 1;
                    break;
                case 1:
                    var products = Database.Database.GetProduct();
                    bool match = false;
                    foreach (var product in products)
                    {
                        if (product.Title.Contains(message.Text))
                        {
                            await client.SendTextMessageAsync(message.Chat, product.prettyPrint());
                            match = true;
                        }
                    }
                    if (!match)
                    {
                        await client.SendTextMessageAsync(message.Chat, "По вашему запросу ничего не найдено");
                    }
                    ModeSearch = false;
                    step = 0;
                    break;
            }
        }

        private static void PriceSearchRun(Message message, TelegramBotClient client)
        {
            switch (step)
            {
                case 0:
                {
                    break;
                }
                case 1:
                {

                    break;
                }
            }
        }
    }
}