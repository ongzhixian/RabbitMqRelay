using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace RabbitMqRelay.ServiceApp.Models
{
    public interface IMessageRouting
    {
        IEnumerable<IEndpoint> Endpoints {get;set;}
        IEnumerable<MessageRoute> MessageRoutes {get;set;}
    }

    public class MessageRouting : IMessageRouting
    {
        public IEnumerable<IEndpoint> Endpoints { get;set;}
        public IEnumerable<MessageRoute> MessageRoutes { get;set;}

        public MessageRouting()
        {
            this.Endpoints = new List<IEndpoint>();
            this.MessageRoutes = new List<MessageRoute>();
        }

        // public static void ReadFromConfiguration(MessageRouting messageRouting)
        // {
        //     // We would want to configure the MessageRouting object from Configuration.
        //     // Unfortunately, we cannot dependency inject IConfiguration object into the method.
        //     // One way is to declare the access modifier of the Configuration object in Program to be 'public'.
        //     // Then we can access the Configuration object like:
        //     // RabbitMqRelay.ServiceApp.Program.Configuration
        //     // 
        // }

        public static MessageRouting ReadFrom(IConfiguration configuration)
        {
            string configPath = $"MessageRouting";

            MessageRouting messageRouting = new MessageRouting();

            IConfigurationSection messageRoutingSection = configuration.GetSection("MessageRouting");

            foreach (IConfigurationSection item in messageRoutingSection.GetChildren())
            {
                switch (item.Key)
                {
                    case "Endpoints":
                        //messageRouting.Endpoints = EndpointsFromConfiguration(item);
                        ConfigureEndpoints(item, messageRouting.Endpoints);
                        //EndpointsFromConfiguration(item, messageRouting.Endpoints);
                        // An alternate way to implement this
                        // messageRouting.Endpoints.FromConfiguration(item);
                        break;
                    case "MessageRoutes":
                        messageRouting.MessageRoutes = MessageRoutesFromConfiguration(item);
                        break;
                    default:
                        break;
                }
            }

            return messageRouting;
        }

        private static void ConfigureEndpoints(IConfigurationSection endPointsSection, IEnumerable<IEndpoint> endpoints)
        {
            var count = endPointsSection.GetChildren().Count();

            foreach (IConfigurationSection endpointSection in endPointsSection.GetChildren())
            {
                bool enabled = endpointSection.GetValue<bool>("Enabled");

                if (!enabled)
                    continue;

                string endpointType = endpointSection.GetValue<string>("Type");

                switch (endpointType)
                {
                    case "SQS":
                        var x = endpointSection.GetValue<string>("Name");

                        break;
                    default:
                        break;
                }
                // foreach (var prop in endpointSection.GetChildren())
                // {
                // }
            }
        }

        private static IEnumerable<Endpoint> EndpointsFromConfiguration(IConfiguration config)
        {
            IEnumerable<Endpoint> result = new List<Endpoint>();

            return result;
        }

        private static IEnumerable<MessageRoute> MessageRoutesFromConfiguration(IConfiguration config)
        {
            IEnumerable<MessageRoute> result = new List<MessageRoute>();

            return result;
        }
    }
}