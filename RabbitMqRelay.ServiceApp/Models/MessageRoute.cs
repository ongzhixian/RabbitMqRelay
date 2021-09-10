using System.Collections.Generic;

namespace RabbitMqRelay.ServiceApp.Models
{
    // "Name" : "Test RabbitMQ to SQS",
    // "Enabled" : true,
    // "SourceEndpoints" : [],
    // "DestinationEndpoints": []

    public interface IMessageRoute
    {
        string Name {get;set;}
        bool Enabled {get;set;}
        IEnumerable<string> SourceEndpoints {get;set;}
        IEnumerable<string> DestinationEndpoints {get;set;}
    }


    public class MessageRoute : IMessageRoute
    {
        public string Name {get;set;}
        public bool Enabled {get;set;}
        public IEnumerable<string> SourceEndpoints {get;set;}
        public IEnumerable<string> DestinationEndpoints {get;set;}

        public MessageRoute()
        {
            this.SourceEndpoints = new List<string>();
            this.DestinationEndpoints = new List<string>();
        }

        public MessageRoute(string name) : this()
        {
            this.Name = name;
        }
    }
}