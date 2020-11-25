using MessagePack;
using MessagePack.Resolvers;
using System;
using System.Diagnostics;
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
                Id = "update1 id",
                UserId = "me",
                IPAddress = "2a00:23c5:82a:cd01:485c:a018:8ef5:b3c3",
                Timestamp = DateTime.Now,
                ObjectId = "object id",
                PropertyName = "Name",
                PreviousValue = "",
                NextValue = "It's me"
            };

            var update1a = update1;
            var update1b = update1 with { NextValue = "asdf" };

            var jsonNS = Newtonsoft.Json.JsonConvert.SerializeObject(update1);
            var jsonSTJ = System.Text.Json.JsonSerializer.Serialize(update1);

            var updateNStoSTJ = System.Text.Json.JsonSerializer.Deserialize<UpdatePropertyEvent>(jsonNS);
            var updateSTJtoNS = Newtonsoft.Json.JsonConvert.DeserializeObject<UpdatePropertyEvent>(jsonSTJ);

            var simple = new SimpleEvent()
            {
                Id = "id",
                IPAddress = "2a00:23c5:82a:cd01:485c:a018:8ef5:b3c3",
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
                IPAddress = "2a00:23c5:82a:cd01:485c:a018:8ef5:b3c3",
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
                EventId = "first event id",
                Name = "Bournemouth",
                Description = "A description",
                BorrowerEntityId = "borrower \"quoted\" id",
                VatReclaimMonths = 1
            };

            var notifier = record.GetNotifier();
            notifier.PropertyUpdated += UpdateReceiver;
            notifier.Name += "!";
            var createEvent = record.GetCreateObjectEvent();
            var newRecord = notifier.GetRecord();
            var createNewEvent = newRecord.GetCreateObjectEvent();

            ViewModelBaseViewNotifier x = null;

            var update2 = new UpdatePropertyEvent()
            {
                Id = "update2 id",
                UserId = "me",
                IPAddress = "2a00:23c5:82a:cd01:485c:a018:8ef5:b3c3",
                Timestamp = DateTime.Now,
                ObjectId = "object id",
                PropertyName = "VatReclaimMonths",
                PreviousValue = "",
                NextValue = "2"
            };

            var record1 = record.ApplyUpdatePropertyEvent(update1);
            var record2 = record.ApplyUpdatePropertyEvent(update2);
            
            void UpdateReceiver(object sender, ViewModelEvent viewModelEvent)
            {
                Console.WriteLine(viewModelEvent);
            }
        }


    }
}
