using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMqRelay.ServiceApp.Services
{
    public interface IMyService
    {
        void DoWork();
    }

    public class MyService : IMyService
    {
        private readonly ILogger<MyService> _logger;

        public MyService(IConfigurationRoot config, ILogger<MyService> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));

            //Console.WriteLine("AAAzzz");
            //Console.WriteLine(config["Test:OptA"]);

            _logger.LogInformation("WWW");
        }

        public void DoWork()
        {
            //Console.WriteLine("Do some work");
            _logger.LogInformation("WWW do some work");
        }

    }

    public class Demo
    {
        private readonly IMyService service;
        public Demo(IMyService service)
        {
            this.service = service;
        }

        public void DoWork()
        {
            service.DoWork();
        }

    }
}
