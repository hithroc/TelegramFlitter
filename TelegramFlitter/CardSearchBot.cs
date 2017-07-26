using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.InlineQueryResults;


namespace TelegramFlitter
{
    class CardSearchBot
    {
        private TelegramBotClient bot;
        private Card[] cards;
        private async void BotOnInlineQuery(object sender, InlineQueryEventArgs args)
        {
            //List<InlineQueryResult> results = new List<InlineQueryResult>();
            //int found = 0;
            Func<Card,InlineQueryResult> consturctResult = card =>
                {
                    int width = 344;
                    int height = 480;
                    if(card.Type == CardType.Problem)
                    {
                        width = 480;
                        height = 344;
                    }
                    return new InlineQueryResultPhoto
                    { ThumbUrl = card.ImageUrl,
                      Url = card.ImageUrl,
                      Id = card.Set + card.Number,
                      ThumbWidth = width,
                      ThumbHeight = height,
                      Width = width,
                      Height = height
                    };
                };
            var results = cards.AsParallel().Where(card => card.FullName.ToLower().Contains(args.InlineQuery.Query.ToLower())).Take(30).Select(consturctResult);

            await bot.AnswerInlineQueryAsync(args.InlineQuery.Id, results.ToArray());
        }

        public CardSearchBot(string token, Card[] cards)
        {
            this.cards = cards;
            bot = new TelegramBotClient(token);
            bot.OnInlineQuery += BotOnInlineQuery;
        }

        public void Run()
        {
            bot.StartReceiving();
        }

        public void Stop()
        {
            bot.StopReceiving();
        }
    }
}
