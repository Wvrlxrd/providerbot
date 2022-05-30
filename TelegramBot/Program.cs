using System;
using System.Collections.Generic;
using System.Linq;
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
        private static bool modeSearch = false;
        private static bool toMatch = false;
        private static int step = 0;
        private static bool modePrice = false;
        private static bool modeAddProduct = false;
        private static bool modeProductCategory = false;
        private static string currentService = "";
        private static string helloString = "Здравствуйте! Я ваш электронный консультант. В нашем боте вы можете:\n" +
                                            "Ознакомиться с асортиментом товаров - /products\n" +
                                            "Ознакомиться с категориями - /category\n" +
                                            "Ознакомиться с услугами - /service\n" +
                                            "Найти желаемый товар - /search\n" +
                                            "Найти по цене - /price";

        private static string adminPassword = "123";
        private static bool isAuth = false;
        private static bool modeRemoveProduct = false;
        private static bool modeUpdateProduct = false;
        private static Dictionary<string, Boolean> _modeDict = new Dictionary<string, bool>();
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
            commands.Add(new PriceCommand());    // Инициализация комманд 
            commands.Add(new AddProductCommand());    // Инициализация комманд 
            commands.Add(new RemoveProductCommand());    // Инициализация комманд 
            commands.Add(new UpdateProductCommand());    // Инициализация комманд 
            commands.Add(new ProblemCommand());    // Инициализация комманд 
            commands.Add(new GetProductByCategoryCommand());    // Инициализация комманд 
            _modeDict.Add("/search", modeSearch);
            _modeDict.Add("/price", modePrice);
            _modeDict.Add("/addProduct", modeAddProduct);
            _modeDict.Add("/removeProduct", modeRemoveProduct);
            _modeDict.Add("/updateProduct", modeUpdateProduct);
            _modeDict.Add("/getProductCategory", modeProductCategory);
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
            
            foreach (var mode in _modeDict)
            {
                Console.WriteLine($"{mode.Key} {mode.Value}");
            }
            Console.WriteLine("Перерождение");
            foreach (var comm in commands)
            {
                // Проверка на наличие комманды в списке
                if (comm.Contains(message.Text))
                {
                    foreach (var mode in _modeDict)
                    {
                        if (mode.Value)
                        {
                            return;
                        } 
                    }
                    foreach (var mode in _modeDict)
                    {
                        Console.WriteLine(mode);
                        if (message.Text == mode.Key)
                        {
                            
                            _modeDict[mode.Key] = true;
                        }
                    }
                    
                    comm.Execute(message, (TelegramBotClient)botClient);
                    toMatch = true;
                }
            }
            
            if (_modeDict["/price"])
            {
                PriceSearchRun(message, (TelegramBotClient)botClient);
            }
            if (_modeDict["/search"])
            {
                SearchRun(message, (TelegramBotClient)botClient);
                return;
            }
            if (_modeDict["/addProduct"])
            {
                AddProduct(message, (TelegramBotClient) botClient);
            }

            if (_modeDict["/removeProduct"])
            {
                RemoveProduct(message, (TelegramBotClient) botClient);
            }
            if (_modeDict["/updateProduct"])
            {
                UpdateProduct(message, (TelegramBotClient) botClient);
            }
            if (_modeDict["/getProductCategory"])
            {
                GetProductsByCategory(message, (TelegramBotClient) botClient);
            }
            if (!toMatch)
            {
                foreach (var mode in _modeDict)
                {
                    if (!mode.Value)
                    {
                        return;
                    } 
                }
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
                    _modeDict["/search"] = false;
                    step = 0;
                    break;
            }
        }

        private static async void PriceSearchRun(Message message, TelegramBotClient client)
        {
            var services = Database.Database.GetService();
            var products = Database.Database.GetProduct();
            var isNotFound = true;
            string introduction = "Выберите услугу:\n";
            
            switch (step)
            {
                case 0:
                {
                    foreach (var service in services)
                    {
                        introduction += service.Title + " ";
                    }
                    await client.SendTextMessageAsync(message.Chat, introduction);
                    
                    step += 1;
                    break;
                }
                case 1:
                {
                    
                    foreach (var service in services)
                    {
                        Console.WriteLine(service.Title.ToLower().Contains(message.Text));
                        if (service.Title.ToLower().Contains(message.Text.ToLower()))
                        {
                            currentService = service.Title;
                            await client.SendTextMessageAsync(message.Chat, "Вы выбрали " + currentService.ToLower());
                            await client.SendTextMessageAsync(message.Chat, "Какой у вас бюджет?");
                            isNotFound = false;
                            step += 1;
                        }
                        
                    }
                    if (isNotFound)
                    {
                        _modeDict["/price"] = false;
                        currentService = "";
                        await client.SendTextMessageAsync(message.Chat, "Цены не найдены");
                        step = 0;
                        return;
                    }
                    break;
                }
                case 2:
                {
                    // TODO: Баг что не выбирается нужная категория
                    var budget = long.Parse(message.Text);
                    long[] array = new long[products.Count];
                    for (int i = 0; i < products.Count; i++)
                    {
                        if (products[i].ServiceTitle == currentService)
                        {
                            array[i] = products[i].Price - budget;    
                        }
                    }
                    
                    int index = Array.IndexOf(array, array.Min());
                    await client.SendTextMessageAsync(message.Chat, $"Вам подходит: {products[index].prettyPrint()}");
                    _modeDict["/price"] = false;
                    currentService = "";
                    step = 0;
                    break;
                }
            }
        }

        private static Product newProduct = new Product(0, "", 100, "", "");
        private static async void AddProduct(Message message, TelegramBotClient client)
        {
            
            var services = Database.Database.GetService();
            switch (step)
            {
                case 0:
                {
                    await client.SendTextMessageAsync(message.Chat, "Введите пароль");
                    
                    step += 1;
                    break;
                }
                case 1:
                {
                    if (message.Text != adminPassword)
                    {
                        _modeDict["/addProduct"] = false;
                        step = 0;
                        await client.SendTextMessageAsync(message.Chat, "Неверный пароль");
                        return;
                    }
                    await client.SendTextMessageAsync(message.Chat, "Введите название");
                    
                    step += 1;
                    break;
                }
                case 2:
                {
                    newProduct.Title = message.Text;
                    await client.SendTextMessageAsync(message.Chat, "Введите цену");
                    
                    step += 1;
                    break;
                }
                
                case 3:
                {
                    long number;
                    bool success = long.TryParse(message.Text, out number);
                    if (!success)
                    {
                        await client.SendTextMessageAsync(message.Chat, "Неверный ввод отмена операции");
                        return;
                    }
                    newProduct.Price = long.Parse(message.Text);

                    await client.SendTextMessageAsync(message.Chat, "Введите описание");   
                    
                    step += 1;
                    break;
                }
                case 4:
                {
                    newProduct.Description = message.Text;
                    await client.SendTextMessageAsync(message.Chat, "Выберите сервис к которому относится продукт");
                    for (int i = 0; i < services.Count; i++)
                    {
                        await client.SendTextMessageAsync(message.Chat, $"{i+1}.{services[i].Title}");
                    }

                    step += 1;
                    break;
                }
                case 5:
                {
                    int id = int.Parse(message.Text);
                    Console.WriteLine(newProduct);
                    Database.Database.AddProduct(newProduct.Title, newProduct.Price, newProduct.Description, services[id-1].Id);
                    newProduct = new Product(0, "", 100, "", "");
                    await client.SendTextMessageAsync(message.Chat, "Продукт успешно добавлен");
                    _modeDict["/addProduct"] = false;
                    step = 0;
                    break;
                }
                    
            }
        }

        private  static async void RemoveProduct(Message message, TelegramBotClient client)
        {
            switch (step)
            {
                case 0:
                {
                    await client.SendTextMessageAsync(message.Chat, "Введите пароль");
                    step += 1;
                    break;
                }
                case 1:
                {
                    if (message.Text != adminPassword)
                    {
                        _modeDict["/addProduct"] = false;
                        await client.SendTextMessageAsync(message.Chat, "Неверный пароль");
                        step = 0;
                        return;
                    }

                    await client.SendTextMessageAsync(message.Chat, "Выберите продукт");
                    new ProductCommand().Execute(message, (TelegramBotClient) botClient);
                    step += 1;
                    break;
                }
                case 2:
                {
                    int choice = int.Parse(message.Text);
                    var products = Database.Database.GetProduct();
                    Database.Database.RemoveProduct(products[choice].Id);
                    await client.SendTextMessageAsync(message.Chat, "Продукт успешно удален");
                    _modeDict["/removeProduct"] = false;
                    step = 0;
                    break;
                }
            }
        }
        static int choice = 0;
        static long id = 0;
        static string title = "";
        static string description = "";
        static long price = 0;
        private static async void UpdateProduct(Message message, TelegramBotClient client)
        {
            
            switch (step)
            {
                case 0:
                {
                    await client.SendTextMessageAsync(message.Chat, "Введите пароль");
                    step += 1;
                    break;
                }
                case 1:
                {
                    if (message.Text != adminPassword)
                    {
                        _modeDict["/updateProduct"] = false;
                        await client.SendTextMessageAsync(message.Chat, "Неверный пароль");
                        step = 0;
                        return;
                    }
                    await client.SendTextMessageAsync(message.Chat, "Выберите позицию");
                    new ProductCommand().Execute(message, (TelegramBotClient)botClient);
                    step += 1;
                    break;
                }
                case 2:
                {
                    choice = int.Parse(message.Text);
                    var product = Database.Database.GetProduct()[choice - 1];
                    id = product.Id;
                    title = product.Title;
                    price = product.Price;
                    description = product.Description;
                    
                    await client.SendTextMessageAsync(message.Chat, "Выберите новое имя продукта. Если не хотите введите 0");
                    step += 1;
                    break;
                }
                case 3:
                {
                    if (message.Text == "0")
                        step += 1;
                    else title = message.Text;
                    step += 1;
                    await client.SendTextMessageAsync(message.Chat, "Выберите новое описание продукта. Если не хотите введите 0");
                    break;
                }
                case 4:
                {
                    await client.SendTextMessageAsync(message.Chat, "Выберите новую цену продукта. Если не хотите введите 0");
                    if (message.Text == "0")
                        step += 1;
                    else description = message.Text;
                    step += 1;
                    break;
                }
                case 5:
                {
                    price = long.Parse(message.Text);
                    await client.SendTextMessageAsync(message.Chat, "Продукт успешно обновлен");
                    // Добавить обновление сервиса
                    Database.Database.UpdateProduct(id, title, price, description);
                    _modeDict["/updateProduct"] = false;
                    step = 0;
                    break;
                }
            }
        }

        private static async void GetProductsByCategory(Message message, TelegramBotClient client)
            {
                int choice = 0;
                switch (step)
                {
                    case 0:
                    {
                        new CategoryCommand().Execute(message, (TelegramBotClient)botClient);
                        await client.SendTextMessageAsync(message.Chat, "Выберите позицию");
                        step += 1;
                        break;
                    }
                    case 1:
                    {
                        choice = int.Parse(message.Text);
                        new ProductCommand().Execute(message, (TelegramBotClient)botClient);
                        step += 1;
                        break;
                    }
                    case 2:
                    {
                        choice = int.Parse(message.Text);
                        var products1 = Database.Database.GetProductByCategory(Database.Database.GetCategory()[choice].Id);
                        foreach (var product in products1)
                        {
                            await client.SendTextMessageAsync(message.Chat, product.prettyPrint());
                        }
                        _modeDict["/getProductCategory"] = false;
                        step = 0;
                        break;
                    }
                }   
            }
        }
    
    
    }