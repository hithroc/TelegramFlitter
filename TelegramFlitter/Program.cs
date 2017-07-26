using System;
using System.IO;
using Newtonsoft.Json;

namespace TelegramFlitter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Card[] cards;
            var def = new { token = "", dbpath = "" };
            var settings = JsonConvert.DeserializeAnonymousType(File.ReadAllText("settings.json"), def);
            using(StreamReader file =  File.OpenText(settings.dbpath))
            {
                JsonSerializer serializer = new JsonSerializer();
                cards = (Card[])serializer.Deserialize(file, typeof(Card[]));
                Console.WriteLine(cards.Length);
            }
            CardSearchBot bot = new CardSearchBot(settings.token, cards);
            bot.Run();
            Console.ReadLine();
            bot.Stop();
        }
    }
}
