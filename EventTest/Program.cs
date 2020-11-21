using MessagePack;
using MessagePack.Resolvers;
using System;
using System.Collections.Immutable;
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

            var update1 = new UpdatePropertyEvent()
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

            var updateNStoSTJ = System.Text.Json.JsonSerializer.Deserialize<UpdatePropertyEvent>(jsonNS);
            var updateSTJtoNS = Newtonsoft.Json.JsonConvert.DeserializeObject<UpdatePropertyEvent>(jsonSTJ);

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

            var buffer = MessagePack.MessagePackSerializer.Serialize(update1, lz4Options);
            var strBuffer = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            var updateMP = MessagePack.MessagePackSerializer.Deserialize<UpdatePropertyEvent>(buffer, lz4Options);

            CreateObjectEvent.PropertyValuePair[] properties =
            {
                new() { PropertyName = "Name", Value = "Me" },
                new() { PropertyName = "Age", Value = "42" },
                new() { PropertyName = "Location", Value = "London" },
            };

            var list = ImmutableList<CreateObjectEvent.PropertyValuePair>.Empty;
            list = list.AddRange(properties);

            CreateObjectEvent create1 = new()
            {
                PartitionKey = "pk",
                Id = "id",
                UserId = "me",
                Timestamp = DateTime.Now,
                TypeDiscriminator = "My Type",
                ObjectId = "object id",
                Properties = properties
            };


            jsonNS = Newtonsoft.Json.JsonConvert.SerializeObject(create1);
            jsonSTJ = System.Text.Json.JsonSerializer.Serialize(create1);

            var createNStoSTJ = System.Text.Json.JsonSerializer.Deserialize<CreateObjectEvent>(jsonNS);
            var createSTJtoNS = Newtonsoft.Json.JsonConvert.DeserializeObject<CreateObjectEvent>(jsonSTJ);
        }
    }
}
