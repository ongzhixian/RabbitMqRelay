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
using RabbitMqRelay.ServiceApp.Models;

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

            // ZX:  When running as as Windows Service, the default run-time directory path (Directory.GetCurrentDirectory()) is 'C:\Windows\System32'
            //      This will be set as the ContentRoot if its not set explicitly.
            //      To avoid inadvertently giving access to this important Windows directory, 
            //      we set the ContentRoot directory to be same directory as the running executable.

            // builder.UseContentRoot(Directory.GetCurrentDirectory());
            string executingDirectory = executingDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? string.Empty;
            builder.UseContentRoot(executingDirectory);

            // This means that any relative file log path, will be based on that location.
            // Setting content root via 'builder.UseContentRoot()' has no impact;

            // ZX:  When running as a Windows Service, any relative file log path defined in Serilog configuration,
            //      will be using 'C:\Windows\System32' as base reference path.
            //      This cannot be changed; setting ContentRoot has no impact on this.
            //      The only workaround is set an absolute path in the Serilog configuration
            //      (which is probably a better practice anyways)

            builder.UseSerilog((hostBuilderContext, loggerConfiguration) =>
             {
                 loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration);
             });

            builder.ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
            {
                //IHostingEnvironment env = hostBuilderContext.HostingEnvironment;
                IHostEnvironment env = hostBuilderContext.HostingEnvironment;
                
                configurationBuilder
                    .AddJsonFile("appsettings.json", optional: false)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                    .AddJsonFile("secret.settings.json", optional: true, reloadOnChange: true);

                Configuration = configurationBuilder.Build();
            });

            builder.ConfigureServices((Action<HostBuilderContext, IServiceCollection>)((hostBuilderContext, services) =>
            {
                services.AddLogging();
                
                services.AddSingleton(Configuration);

                // ZX:  We want do something neater like the below.
                //      But we cannot get Configuration object (unless we make it 'public').
                //      While there are probably good reasons to make Configuration, 
                //      let's go with current setup where Configuration is static to Program class.
                // services.Configure<MessageRouting>(MessageRouting.ReadFromConfiguration);

                // So we configure MessageRouting by passing Configuration object to helper method.
                
                services.Configure<MessageRouting>(messageRouting => {
                    messageRouting = MessageRouting.ReadFrom(Configuration);
                });
                

                // Singleton: IoC container will create and share a single instance of a service throughout the application's lifetime.
                // Transient: The IoC container will create a new instance of the specified service type every time you ask for it.
                // Scoped: IoC container will create an instance of the specified service type once per request and will be shared in a single request.

                //services.AddHostedService<Worker>();
                
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

            builder.UseWindowsService();

            return builder;
        }

    }
}
