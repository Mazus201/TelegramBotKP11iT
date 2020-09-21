using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks.Dataflow;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using ApiAiSDK;
using ApiAiSDK.Model;

namespace KP11_Bot
{
    class Program
    {

        /// <summary>
        /// тело бота
        /// </summary>
        private static ITelegramBotClient botClient;
        static ApiAi apiAi;
        static void Main(string[] args)
        {
            botClient = new TelegramBotClient("1227072027:AAGGhpJp7UvgEzrVuFq4Msof92rFHHaOTk4") { Timeout = TimeSpan.FromSeconds(10) }; 
                                                                                    //подключение к АПИ и разгрузка сервера
            AIConfiguration config = new AIConfiguration("AIzaSyCVtgn4Sk0BeoUVYkqkPAxNF2llPI9tZ0I", SupportedLanguage.Russian);
            apiAi = new ApiAi(config);

            var me = botClient.GetMeAsync().Result;                                 //Получение данных о боте 

            Console.WriteLine($"Bot id: {me.Id}. Bot name is {me.FirstName}");      //подключен ли бот и как его зовут

            botClient.OnMessage += Bot_OnMessage;                                   //подписались на получение сообщения
            botClient.OnCallbackQuery += BotOnCallbackQueryRecived;                 //подключили обработку действий с кнопок

            botClient.StartReceiving();                                             //ожидание новых сообщений 

            Console.ReadKey();                                                      //консоль не будет закрываться)))
        }

        /// <summary>
        /// действия при нажатии на пунты меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static async void BotOnCallbackQueryRecived(object sender, CallbackQueryEventArgs e)
        {
            string buttonText = e.CallbackQuery.Data;                                           //получем данные из названия кнопки
            string name = $"{e.CallbackQuery.From.FirstName} {e.CallbackQuery.From.LastName}";  //достаем имя пользователя
            Console.WriteLine($"{name} нажал {buttonText}");                                    //отображаем в консоли что выбрал юхер

            switch(buttonText)
            {
                case "Где столовая?":
                    await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, "https://youtu.be/crnClMC1wec"); //выводим текст, если выбрана столовая
                break;

                case "Где актовый зал?":
                    await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, 
                        "Сначала ты покушай, а представления потом");

                    break;
                case "Расписание":
                    await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, 
                        "https://vk.com/doc214376375_570560036?hash=83dfae16db77607f67&dl=8a44a9bebd840d54ed"); 

                    break;
                case "Где мой кабинет?":
                    await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, 
                        "Твой кабинет в колледже"); 

                    break;
                case "Меню в столовой":
                    await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, 
                        "https://sch883sz.mskobr.ru/files/ss21092020.pdf"); 

                    break;
                case "Связь с куратором":
                    await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, 
                        "Тут будет ссылка на преподавателя нужного"); 

                    break;
                case "Связь с":
                    await botClient.SendTextMessageAsync(e.CallbackQuery.From.Id, 
                        "Я еще не придумал");

                    break;
            }

            try
            {
                await botClient.AnswerCallbackQueryAsync(e.CallbackQuery.Id, $"Вы выбрали {buttonText}"); //отображаем юзеру, что он выбрал
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }




        /// <summary>
        /// само сообщение и обработка текста
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            var message = e.Message;                                                 //сохраняем введеный поьзователем текст
            string nameOfUser = $"{message.From.FirstName} {message.From.LastName}"; //достаем имя пользователя
            string groupOfUser = null;                                               //храним группу пользователя (пока хз как)

            if (message == null || message.Type != MessageType.Text)                 //проверяем на тип отправленных данных пользователем
            {
                Console.WriteLine($"Пользователь {nameOfUser} прислал не текст");    //предупреждение о некорректности ввденых данных
                return; //покидаем метод
            }

            Console.WriteLine($"{nameOfUser} отправил сообщение: '{message.Text}'"); //инфа для нас о пользователях и их сообщениях

            switch(message.Text)                                                     //обработка команд введеных пользователями
            {
                case "/start":
                    string text =                                                    //переменная для вывода текста пользователю
@$"Привет, {nameOfUser}! 
Теперь ты часть нашего лампового сообщества, рад, что ты присоединился :)
Для того чтобы я мог корректно работать, пришли мне из какой ты группы, пожалуйста.
Кстати, ты можешь ознакмиться со списком комманд, которые я умею выполнять, написав '/help'";
                    await botClient.SendTextMessageAsync(message.From.Id, text);     //вывод самого сообщения
                    break;

                case "/internet":
                    var menuInternet = new InlineKeyboardMarkup(new[]                //создание меню с кнопками
                    {
                        new[]                                                        //делаем двумерный массив и получаем два ряда и два столбца кнопок. Это первый ряд
                        {
                            InlineKeyboardButton.WithUrl("VK", "https://vk.com/gapoukp11"),
                            InlineKeyboardButton.WithUrl("Instagram", "http://instagram.com/vseokp11"),
                            InlineKeyboardButton.WithUrl("WhatsUp", "https://api.whatsapp.com/send?phone=796721993902")
                        },
                        new[] //это второй ряд
                        {
                            InlineKeyboardButton.WithUrl("Сайт", "https://www.kp11.ru/"),
                            InlineKeyboardButton.WithUrl("YouTube", "https://www.youtube.com/user/kp11ru/feed?filter=2"),
                            InlineKeyboardButton.WithUrl("FaceBook", "https://www.facebook.com/gapoukp11")
                        }
                    }
                    );

                    await botClient.SendTextMessageAsync(message.From.Id, "Мы есть в интернете! Можешь найти по следующим ссылкам :)", replyMarkup: menuInternet); //вывод кнопок на экран
                    break;

                case "/keyboard":
                    var replaceKeyboard = new ReplyKeyboardMarkup(new[]                 //создаем кнопки для "клавиатуры". Это двумерный массив
                    {
                        new[]                                                           //первый ряд кнопок клавиатуры
                        {
                            new KeyboardButton("Поделиться геолокацией") {RequestLocation = true},
                            new KeyboardButton("Поделиться контактом") {RequestContact = true}
                        },
                        new[]                                                           //второй ряд клавиатуры
                        {
                            new KeyboardButton("Где столовая?"),
                            new KeyboardButton("Где актовывй зал?")
                        }
                    });
                    await botClient.SendTextMessageAsync(message.From.Id, "Выбери, что ты хочешь узнать:", replyMarkup: replaceKeyboard);
                                                                                        //обрабатывем действие юхера и выводим клавиатуру
                    break;

                case "/menu": //меню действий для студента
                    var menuDo = new InlineKeyboardMarkup(new[]                         //делаем кнопки под сообщением. Опять многомерный массив
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Расписание"),
                            InlineKeyboardButton.WithCallbackData("Где мой кабинет?"),
                            InlineKeyboardButton.WithCallbackData("Меню в столовой")
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Где столовая?"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Где актовый зал?"),
                            InlineKeyboardButton.WithCallbackData("Связь с куратором"),
                            InlineKeyboardButton.WithCallbackData("Связь с ")
                        }
                    }
                    );

                    await botClient.SendTextMessageAsync(message.From.Id, "Что ты хотел узнать?", replyMarkup: menuDo); //выводим эти кнопки

                    break;

                case "/help":                                                           //основыне команды для пользователя
string textHelp = //описываем действие на ХЕЛП
@$"Для навигации по боту ты можешь использовать следующие команды:
/start - для вызова первого сообщения от меня;
/menu - для вызова моих основных функций;
/internet - это ссылки на нас в интернете;
/keyboard - для удобного доступа к командам;
/help - для вызова этого меню;";
                    await botClient.SendTextMessageAsync(message.From.Id, textHelp);

                    break;

                default: //это остальное. Тут должна быть обработка сообщений - не команд (обычных сообщений боту), которая осуществляется через апи DialogFlow
                    try
                    {
                        var responce = apiAi.TextRequest(message.Text);                      //Текст из переменной отправляем на сервер АПИ
                        string answer = responce.Result.Fulfillment.Speech;                  //получаем ответ на текст, отправленный на АПИ
                        if (answer == "")
                            answer = "Прости, я не понял, что ты имел ввиду";                //если поступила неизвестная команда
                        await botClient.SendTextMessageAsync(message.From.Id, answer);       //отвечаем пользователю ответом от АПИ
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
            }

            //if (text == null)
            //    return;

            //await botClient.SendTextMessageAsync(
            //    chatId: e.Message.Chat,
            //    text: $"Рад, что ты присоединился! Как тебя зовут?"
            //    ).ConfigureAwait(false);

        }



    }
}
