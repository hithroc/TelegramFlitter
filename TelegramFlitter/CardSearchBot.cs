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
            Func<Card,InlineQueryResult> consturctResult = card =>
                {
                    int width  = card.Type != CardType.Problem ? 344 : 480;
                    int height = card.Type != CardType.Problem ? 480 : 344;
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
