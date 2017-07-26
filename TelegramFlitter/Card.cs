using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TelegramFlitter
{

    enum CardType
    {
        Problem,
        Mane,
        Troublemaker,
        Resource,
        Event,
        Friend
    }

    struct Card
    {
        private static readonly string urlPrefix = "http://ponyhead.com/img/cards/";
        private static readonly string urlPostfix = ".jpg";
        public string Title { get; }
        public string SubTitle { get; }
        public string Set { get; }
        public string Number { get; }
        public string FullName { get => Title + (SubTitle == "" ? "" : ", " + SubTitle); }
        public CardType Type { get; }
        public string ImageUrl
        {
            get
            {
                var id = Set.ToLower() + Number.ToUpper();
                // Hardcoded ponyhead mappings
                if(Set == "PR" && Number == "212")
                    id =  "prPF1";
                if(Set == "PR" && Number == "213")
                    id = "prPF2";
                if(Set == "CN" && Number == "f2")
                    id = "cnf2";
                if(Set == "CN" && Number == "f4")
                    id = "cnf4";
                if(Set == "CS" && Number == "f1")
                    id = "csf1";
                if(Set == "CG" && Number == "pf11")
                    id = "cgpf11";
                if(Set == "CG" && Number == "pf8")
                    id = "cspf8";
                if(Set == "AD" && Number == "d1")
                    id = "add1";
                if(Set == "AD" && Number == "f10")
                    id = "adf10";
                if(Set == "AD" && Number == "f8")
                    id = "adf8";
                if(Set == "AD" && Number == "f6")
                    id = "adf6";
                if(Set == "AD" && Number == "f12")
                    id = "adf12";
                if(Set == "AD" && Number == "f4")
                    id = "adf4";
                return urlPrefix + id + urlPostfix;
            }
        }

        public Card(string title, string subtitle, string set, string number, CardType type)
        {
            Title = title;
            SubTitle = subtitle;
            Set = set;
            Number = number;
            Type = type;
        }

    }

    class CardConverter : JsonConverter
    {
        public override bool CanWrite { get => false; }
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Card);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);
            var title = (string)obj["title"];
            var subtitle = (string)obj["subtitle"];
            var type = (string)obj["type"];
            var set = (string)obj["set"];
            var number = (string)obj["number"];
            return new Card(title, subtitle, set, number, (CardType)Enum.Parse(typeof(CardType), type));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
