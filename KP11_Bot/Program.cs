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

namespace KP11_Bot
{
    class Program
    {

        /// <summary>
        /// тело бота
        /// </summary>
        private static ITelegramBotClient botClient;
        static void Main(string[] args)
        {
            botClient = new TelegramBotClient("1227072027:AAGGhpJp7UvgEzrVuFq4Msof92rFHHaOTk4") { Timeout = TimeSpan.FromSeconds(10) }; //подключение к АПИ и разгрузка сервера
            var me = botClient.GetMeAsync().Result;//Получение данных о боте

            Console.WriteLine($"Bot id: {me.Id}. Bot name is {me.FirstName}");//подключен ли бот и как его зовут

            botClient.OnMessage += Bot_OnMessage; //подписались на получение сообщения
            botClient.StartReceiving(); //ожидание новых сообщений 

            Console.ReadKey();//консоль не будет закрываться)))
        }

        /// <summary>
        /// действия при нажатии на пунты меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void BotOnCallbackQueryRecived(object sender, CallbackQueryEventArgs e)
        {

        }

        /// <summary>
        /// само сообщение и обработка текста
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            var message = e.Message; //сохраняем введеный поьзователем текст
            string nameOfUser = $"{message.From.FirstName} {message.From.LastName}"; //достаем имя пользователя
            string groupOfUser = null; //храним группу пользователя (пока хз как)

            if (message == null || message.Type != MessageType.Text) //проверяем на тип отправленных данных пользователем
            {
                Console.WriteLine($"Пользователь {nameOfUser} прислал не текст"); //предупреждение о некорректности ввденых данных
                return; //покидаем метод
            }

            Console.WriteLine($"{nameOfUser} отправил сообщение: '{message.Text}'"); //инфа для нас о пользователях и их сообщениях

            switch(message.Text) //обработка команд введеных пользователями
            {
                case "/start":
                    string text = //переменная для вывода текста пользователю
@$"Привет, {nameOfUser}! 
Теперь ты часть нашего лампового сообщества, рад, что ты присоединился :)
Для того чтобы я мог корректно работать, пришли мне из какой ты группы, пожалуйста.
Кстати, ты можешь ознакмиться со списком комманд, которые я умею выполнять, написав '/help'";
                    await botClient.SendTextMessageAsync(message.From.Id, text); //вывод самого сообщения
                    break;

                case "/menu":
                    var menuKeyboard = new InlineKeyboardMarkup(new[] //создание меню с кнопками
                    {
                        new[] //делаем двумерный массив и получаем два ряда и два столбца кнопок. Это первый ряд
                        {
                            InlineKeyboardButton.WithUrl("VK", "https://vk.com/gapoukp11"),
                            InlineKeyboardButton.WithUrl("Instagram", "http://instagram.com/vseokp11")
                        },
                        new[] //это второй ряд
                        {
                            InlineKeyboardButton.WithCallbackData("Пункт 1"),
                            InlineKeyboardButton.WithCallbackData("Пункт 2")
                        }
                    }
                    );

                    await botClient.SendTextMessageAsync(message.From.Id, "Выберите пункт", replyMarkup: menuKeyboard); //вывод кнопок на экран
                    break;

                case "/keyboard":

                    break;

                case "/help":
string textHelp = //описываем действие на ХЕЛП
@$"Для навигации по боту ты можешь использовать следующие команды:
/start - для вызова первого сообщения от меня;
/menu - для вывзова меню с выбором действий;
/keyboard - пока не понял для чего));
/help - для вызова этого меню;
";
                    await botClient.SendTextMessageAsync(message.From.Id, textHelp);
                    break;

                default: //это остальное
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
