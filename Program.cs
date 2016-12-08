using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Newtonsoft.Json;


namespace SimpleEventStoreDemo
{
    class Program
    {

        static string StreamId(Guid id)
        {
            return String.Format("BankAccount-{0}", id.ToString());
        }

        static void Main(string[] args)
        {

            IEventStoreConnection connection = EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback, 1113));
            connection.ConnectAsync();

            var aggregateId = Guid.NewGuid();
            List<IEvent> eventsToRun = new List<IEvent>();

            //Commands
            //Domain Logic/Model
            //Events

            eventsToRun.Add(new AccountCreatedEvent(aggregateId, "Greg Chavez"));
            eventsToRun.Add(new FundsDespoitedEvent(aggregateId, 150));
            eventsToRun.Add(new FundsDespoitedEvent(aggregateId, 100));
            eventsToRun.Add(new FundsWithdrawedEvent(aggregateId, 60));
            eventsToRun.Add(new FundsWithdrawedEvent(aggregateId, 94));
            eventsToRun.Add(new FundsDespoitedEvent(aggregateId, 4)); //100

            foreach (var item in eventsToRun)
            {
                var jsonString = JsonConvert.SerializeObject(item, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                var jsonPayload = Encoding.UTF8.GetBytes(jsonString);
                var eventStoreDataType = new EventData(Guid.NewGuid(), item.GetType().Name, true, jsonPayload, null);
                connection.AppendToStreamAsync(StreamId(aggregateId), ExpectedVersion.Any, eventStoreDataType);
            }

            var results = Task.Run(() => connection.ReadStreamEventsForwardAsync(StreamId(aggregateId), StreamPosition.Start, 999,
            false));
            Task.WaitAll();

            var resultsData = results.Result;

            var bankState = new BankAccount();

            foreach (var evnt in resultsData.Events)
            {
                var esJsonData = Encoding.UTF8.GetString(evnt.Event.Data);
                if (evnt.Event.EventType == "AccountCreatedEvent")
                {
                    var objState = JsonConvert.DeserializeObject<AccountCreatedEvent>(esJsonData);
                    bankState.Apply(objState);

                }
                else if (evnt.Event.EventType == "FundsDespoitedEvent")
                {
                    var objState = JsonConvert.DeserializeObject<FundsDespoitedEvent>(esJsonData);
                    bankState.Apply(objState);
                }
                else
                {
                    var objState = JsonConvert.DeserializeObject<FundsWithdrawedEvent>(esJsonData);
                    bankState.Apply(objState);
                }
            } 

            Console.ReadLine();
        }
    }
}
