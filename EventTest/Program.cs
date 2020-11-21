using MessagePack;
using MessagePack.Resolvers;
using System;
using System.Text;
using Vectis.Events;

namespace EventTest
{
    class Program
    {
        [MessagePack.MessagePackObject]
        public record SimpleEvent : Event
        {
            public SimpleEvent() { }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var update1 = new UpdatePropertyEvent<string>()
            {
                PartitionKey = "pk",
                Id = "id",
                UserId = "me",
                Timestamp = DateTime.Now,
                ObjectId = "object id",
                PropertyName = "Name",
                PreviousValue = "",
                NextValue = "It's me"
            };

            var update2 = update1;
            var update3 = update1 with { NextValue = "asdf" };

            var jsonNS = Newtonsoft.Json.JsonConvert.SerializeObject(update1);
            var jsonSTJ = System.Text.Json.JsonSerializer.Serialize(update1);

            var updateNStoSTJ = System.Text.Json.JsonSerializer.Deserialize<UpdatePropertyEvent<string>>(jsonNS);
            var updateSTJtoNS = Newtonsoft.Json.JsonConvert.DeserializeObject<UpdatePropertyEvent<string>>(jsonSTJ);

            var simple = new SimpleEvent()
            {
                PartitionKey = "pk",
                Id = "id",
                UserId = "me",
                Timestamp = DateTime.Now
            };

            var resolver = MessagePack.Resolvers.CompositeResolver.Create(
                StandardResolver.Instance
            );

            var lz4Options = MessagePackSerializerOptions
                .Standard
                .WithResolver(resolver)
                .WithCompression(MessagePackCompression.Lz4BlockArray);

            var buffer = MessagePack.MessagePackSerializer.Typeless.Serialize(update1);
            var strBuffer = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            var updateMP = MessagePack.MessagePackSerializer.Deserialize<UpdatePropertyEvent<string>>(buffer);
        }
    }
}
