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
        public record SimpleEvent : ViewModelEvent
        {
            public SimpleEvent() { }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var update1 = new UpdatePropertyEvent()
            {
                Id = "id",
                UserId = "me",
                IPAddressString = "2a00:23c5:82a:cd01:485c:a018:8ef5:b3c3",
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
                Id = "id",
                IPAddressString = "2a00:23c5:82a:cd01:485c:a018:8ef5:b3c3",
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

            CreateObjectEvent create1 = new()
            {
                Id = "id",
                IPAddressString = "2a00:23c5:82a:cd01:485c:a018:8ef5:b3c3",
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

            //var runner = new Runner();
            //runner.Run("");

            //_ = runner;

            var record = new SchemeRecord()
            {
                Id = "scheme id",
                Name = "Bournemouth",
                Description = "A description",
                BorrowerEntityId = "borrower id",
                VatReclaimMonths = 1
            };

            var notifier = record.GetNotifier();
            notifier.Name += "!";
            ViewModelBaseViewNotifier x;
            var createEvent = record.GetCreateObjectEvent("me!");

            MyDerived derived = new()
            {
                Id = "id",
                Name = "name"
            };

            var d2 = derived.Update("Name", "new name");
            var d3 = derived.Update("Id", "new id");
        }


        public abstract record MyBase
        {
            public string Id { get; init; }

            public virtual MyBase Update(string property, string value)
            {
                return property switch
                {
                    "Id" => this with { Id = value },
                    _ => throw new Exception()
                };
            }
        }

        public record MyDerived : MyBase
        {
            public string Name { get; init; }

            public override MyDerived Update(string property, string value)
            {
                return property switch
                {
                    "Name" => this with { Name = value },
                    _ => base.Update(property, value) as MyDerived
                };
            }
        }
    }
}
