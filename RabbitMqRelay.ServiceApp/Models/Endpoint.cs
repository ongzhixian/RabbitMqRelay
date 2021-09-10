namespace RabbitMqRelay.ServiceApp.Models
{
    public interface ISettings
    {
    }

    public class RabbitMqSettings : ISettings
    {
    }

    public interface IEndpoint
    {
        string Name {get;set;}
        bool Enabled {get;set;}
        string Type {get;set;}
        ISettings Settings {get;set;}
    }

    // "Name" : "SQS xxxx",
    // "Enabled" : false,
    // "Type" : "SQS",
    // "Settings" : "",
    // "Configuration" : ""

    public class Endpoint : IEndpoint
    {
        public string Name {get;set;}
        public bool Enabled {get;set;}
        public string Type {get;set;}
        public ISettings Settings {get;set;}

        public Endpoint()
        {

        }
    }
}