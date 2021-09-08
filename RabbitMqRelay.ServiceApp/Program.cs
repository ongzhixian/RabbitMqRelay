using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using RabbitMqRelay.ServiceApp.Services;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;


namespace RabbitMqRelay.ServiceApp
{
    class Program
    {
        static IConfigurationRoot Configuration { get; set; }

        static async Task Main(string[] args)
        {
            using (ILoggerFactory loggerFactory = new LoggerFactory())
            using (IHost host = CreateHostBuilder(args).Build())
            {
                IServiceProvider services = host.Services;

                ILogger log = services.GetRequiredService<ILogger<Program>>();

                log.LogInformation("Application start");

                try
                {
                    await host.RunAsync();
                }
                catch (Exception ex)
                {
                    log.LogError(ex, string.Empty);
                    throw;
                }
            }

            
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            // Notes: .NET Core 2.1 does not have a CreateDefaultBuilder
            // See the below github as a reference
            // https://github.com/dotnet/runtime/blob/102fc35e821a983ab6cab87e25322eb950b07d6b/src/libraries/Microsoft.Extensions.Hosting/src/HostingHostBuilderExtensions.cs#L190

            HostBuilder builder = new HostBuilder();

            builder.UseContentRoot(Directory.GetCurrentDirectory());

            builder.UseSerilog((hostBuilderContext, loggerConfiguration) =>
             {
                 loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration);
             });

            builder.ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
            {
                //IHostingEnvironment env = hostBuilderContext.HostingEnvironment;
                IHostEnvironment env = hostBuilderContext.HostingEnvironment;
                
                configurationBuilder
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                    .AddJsonFile("secret.settings.json", optional: true, reloadOnChange: true);

                Configuration = configurationBuilder.Build();
            });

            builder.ConfigureServices((Action<HostBuilderContext, IServiceCollection>)((hostBuilderContext, services) =>
            {
                //services.AddHostedService<Worker>();
                services.AddLogging();
                services.AddSingleton(Configuration);
                services.AddSingleton<IMyService, MyService>();
                services.AddHostedService<ExampleHostedService>();

                //services.AddSingleton<IMessagingService, RabbitMqService>();

                //services.AddSingleton<IMessagingService, AwsMessagingService>();

                // services.AddSingleton<IMessagingService, NmsMessagingService>();

                // services.AddSingleton<IMoldUdp64Server, MoldUdp64Server>(sp =>
                // {
                //     //ILogger<MoldUdp64Server> log = sp.GetRequiredService<ILogger<MoldUdp64Server>>();
                //     //string ipAddress = Configuration["MoldUdp64:IP_Address"];
                //     //int ipPort = int.Parse(Configuration["MoldUdp64:IP_Port"]);
                //     return new MoldUdp64Server(
                //         Configuration["MoldUdp64:IP_Address"],
                //         int.Parse(Configuration["MoldUdp64:IP_Port"]),
                //         sp.GetRequiredService<ILogger<MoldUdp64Server>>());
                // });

                // services.AddSingleton<IMarketDataProtocolServer, MarketDataProtocolServer>(sp =>
                // {
                //     //ILogger<MoldUdp64Server> log = sp.GetRequiredService<ILogger<MoldUdp64Server>>();
                //     //string ipAddress = Configuration["MoldUdp64:IP_Address"];
                //     //int ipPort = int.Parse(Configuration["MoldUdp64:IP_Port"]);
                //     return new MarketDataProtocolServer(
                //         Configuration["MoldUdp64:IP_Address"],
                //         int.Parse(Configuration["MoldUdp64:IP_Port"]),
                //         sp.GetRequiredService<ILogger<MarketDataProtocolServer>>());
                // });

            }));

            return builder;
        }

    }
}
