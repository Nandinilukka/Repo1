
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using MQTTnet.Client.Options;
//using MQTTnet.Client;
//using MQTTnet;
//using NServiceBus;
//using System.Text;


//namespace BuisinessLogicLayer
//{
//    public class Program
//    {
//        static async Task Main(string[] args)
//        {

//            Console.Title = "ClientUI";

//            var builder = Host.CreateApplicationBuilder(args);

//            var endpointConfiguration = new EndpointConfiguration("ClientUI");
//            endpointConfiguration.UseSerialization<SystemJsonSerializer>();
//            endpointConfiguration.EnableInstallers();

//            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
//            transport.ConnectionString("host=localhost;username=guest;password=guest");
//            transport.UseConventionalRoutingTopology(QueueType.Quorum);

//            builder.UseNServiceBus(endpointConfiguration);

//            // Register MQTT client as a singleton service
//            builder.Services.AddSingleton<IMqttClient>(provider =>
//            {
//                var factory = new MqttFactory();
//                var mqttClient = factory.CreateMqttClient();
//                return mqttClient;
//            });
//            builder.Services.AddHostedService<InputLoopService>();


//            var app = builder.Build();
//            await app.RunAsync();
//        }
//    }
//}


//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using MQTTnet.Client.Options;
//using MQTTnet.Client;
//using MQTTnet;
//using NServiceBus;
//using System.Text;
//using DataAccessLayer.Model;

//namespace BuisinessLogicLayer
//{
//    public class Program
//    {
//        static async Task Main(string[] args)
//        {
//            Console.Title = "ClientUI";

//            var builder = Host.CreateApplicationBuilder(args);

//            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

//            var endpointConfiguration = new EndpointConfiguration("ClientUI");
//            endpointConfiguration.UseSerialization<SystemJsonSerializer>();
//            endpointConfiguration.EnableInstallers();

//            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
//            transport.ConnectionString("host=localhost;username=guest;password=guest");
//            transport.UseConventionalRoutingTopology(QueueType.Quorum);

//            builder.UseNServiceBus(endpointConfiguration);

//            // Register MQTT client as a singleton service
//            builder.Services.AddSingleton<IMqttClient>(provider =>
//            {
//                var mqttSettings = provider.GetRequiredService<IConfiguration>().GetSection("MqttSettings").Get<MqttSettings>();

//                var factory = new MqttFactory();
//                var mqttClient = factory.CreateMqttClient();

//                var mqttOptionsBuilder = new MqttClientOptionsBuilder()
//                    .WithClientId(mqttSettings.ClientId)
//                    .WithTcpServer(mqttSettings.BrokerAddress, mqttSettings.BrokerPort)
//                    .WithCleanSession(mqttSettings.CleanSession)
//                    .WithKeepAlivePeriod(TimeSpan.FromSeconds(mqttSettings.KeepAlivePeriod));

//                var mqttOptions = mqttOptionsBuilder.Build();
//                mqttClient.ConnectAsync(mqttOptions).GetAwaiter().GetResult();  // Connect to the MQTT broker

//                return mqttClient;
//            });

//            builder.Services.AddHostedService<InputLoopService>();

//            var app = builder.Build();
//            await app.RunAsync();
//        }
//    }
//}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MQTTnet.Client.Options;
using MQTTnet.Client;
using MQTTnet;
using NServiceBus;
using System.Text;
using DataAccessLayer.Model;

namespace BuisinessLogicLayer
{
/// <summary>
/// 
/// </summary>
    public class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "ClientUI";
                
            var builder = Host.CreateApplicationBuilder(args);

            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var endpointConfiguration = new EndpointConfiguration("ClientUI");
            endpointConfiguration.UseSerialization<SystemJsonSerializer>();
            endpointConfiguration.EnableInstallers();

            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.ConnectionString("host=localhost;username=guest;password=guest");
            transport.UseConventionalRoutingTopology(QueueType.Quorum);

            builder.UseNServiceBus(endpointConfiguration);

            builder.Services.AddSingleton<IMqttClient>(provider =>
            {
                var mqttSettings = provider.GetRequiredService<IConfiguration>().GetSection("MqttSettings").Get<MqttSettings>();

                var factory = new MqttFactory();
                var mqttClient = factory.CreateMqttClient();
                return mqttClient;
            });

            builder.Services.AddHostedService<InputLoopService>();

            var app = builder.Build();
            await app.RunAsync();
        }
    }
}
