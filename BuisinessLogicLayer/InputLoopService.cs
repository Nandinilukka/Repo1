
//using MQTTnet;
//using MQTTnet.Client;
//using System;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.Extensions.Configuration;
//using NServiceBus;
//using BuisinessLogicLayer.RabbitMqCommands;
//using Microsoft.Extensions.Hosting;
//using MQTTnet.Client.Options;

//public class InputLoopService : BackgroundService
//{
//    private readonly IMessageSession _messageSession;
//    private readonly IMqttClient _mqttClient;
//    private readonly IConfiguration _configuration;

//    private string _brokerAddress;
//    private int _brokerPort;
//    private string[] _topics;

//    public InputLoopService(IMessageSession messageSession, IMqttClient mqttClient, IConfiguration configuration)
//    {
//        _messageSession = messageSession;
//        _mqttClient = mqttClient;
//        _configuration = configuration;

//        // Read MQTT settings from configuration
//        var mqttSettings = _configuration.GetSection("MqttSettings");
//        _brokerAddress = mqttSettings.GetValue<string>("BrokerAddress");
//        _brokerPort = mqttSettings.GetValue<int>("BrokerPort");
//        _topics = mqttSettings.GetSection("Topics").Get<string[]>();
//    }

//    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    {
//        // Subscribe to MQTT topics when the service starts
//        await SubscribeToMqttMessages(stoppingToken);

//        while (!stoppingToken.IsCancellationRequested)
//        {
//            Console.WriteLine("Press 'C' to create a product, or 'Q' to quit.");
//            var key = Console.ReadKey();
//            Console.WriteLine();

//            switch (key.Key)
//            {
//                case ConsoleKey.C:
//                    var command = new CreateProductCommand
//                    {
//                        ProductName = "New Product",
//                        Quantity = 10
//                    };

//                    Console.WriteLine($"Sending CreateProductCommand, ProductName = {command.ProductName}, Quantity = {command.Quantity}");
//                    await _messageSession.SendLocal(command, stoppingToken);
//                    break;

//                case ConsoleKey.Q:
//                    //// Gracefully stop the MQTT client before exiting
//                    //await DisconnectMqttClient();
//                    return;

//                default:
//                    Console.WriteLine("Unknown input. Please try again.");
//                    break;
//            }
//        }
//    }

//    private async Task SubscribeToMqttMessages(CancellationToken stoppingToken)
//    {
//        // Check if already connected before attempting to connect
//        if (!_mqttClient.IsConnected)
//        {
//            var mqttOptions = new MqttClientOptionsBuilder()
//                .WithClientId(Guid.NewGuid().ToString())
//                .WithTcpServer(_brokerAddress, _brokerPort)
//                .WithCleanSession()
//                .Build();

//            await _mqttClient.ConnectAsync(mqttOptions, stoppingToken);
//            Console.WriteLine($"Connected to MQTT broker at {_brokerAddress}:{_brokerPort}");
//        }

//        // Set up MQTT message received handler
//        _mqttClient.UseApplicationMessageReceivedHandler(e =>
//        {
//            var topic = e.ApplicationMessage.Topic;
//            var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
//            Console.WriteLine($"Received MQTT message: Topic = {topic}, Message = {message}");
//        });
//        _mqttClient.Options.
//        // Subscribe to topics from the configuration
//        foreach (var topic in _topics)
//        {
//            await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());
//            var message = Encoding.UTF8.GetString(topic..ApplicationMessage.Payload);
//            Console.WriteLine($"Received MQTT message: Topic = {topic}, Message = {message}");
//            Console.WriteLine($"Subscribed to MQTT topic: {topic}");
//        }

//    }


//}




using MQTTnet;
using MQTTnet.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NServiceBus;
using BuisinessLogicLayer.RabbitMqCommands;
using Microsoft.Extensions.Hosting;
using MQTTnet.Client.Options;

public class InputLoopService : BackgroundService
{
    private readonly IMessageSession _messageSession;
    private readonly IMqttClient _mqttClient;
    private readonly IConfiguration _configuration;

    private string _brokerAddress;
    private int _brokerPort;
    private string[] _topics;

    public InputLoopService(IMessageSession messageSession, IMqttClient mqttClient, IConfiguration configuration)
    {
        _messageSession = messageSession;
        _mqttClient = mqttClient;
        _configuration = configuration;

        var mqttSettings = _configuration.GetSection("MqttSettings");
        _brokerAddress = mqttSettings.GetValue<string>("BrokerAddress");
        _brokerPort = mqttSettings.GetValue<int>("BrokerPort");
        _topics = mqttSettings.GetSection("Topics").Get<string[]>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await SubscribeToMqttMessages(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("Press 'C' to create a product, or 'Q' to quit.");
            var key = Console.ReadKey();
            Console.WriteLine();

            switch (key.Key)
            {
                case ConsoleKey.C:
                    var command = new CreateProductCommand
                    {
                        ProductName = "New Product",
                        Quantity = 10
                    };

                    Console.WriteLine($"Sending CreateProductCommand, ProductName = {command.ProductName}, Quantity = {command.Quantity}");
                    await _messageSession.SendLocal(command, stoppingToken);
                    break;

                case ConsoleKey.Q:                 
                    return;

                default:
                    Console.WriteLine("Unknown input. Please try again.");
                    break;
            }
        }
    }

    private async Task SubscribeToMqttMessages(CancellationToken stoppingToken)
    {
        if (!_mqttClient.IsConnected)
        {
            var mqttOptions = new MqttClientOptionsBuilder()
                .WithClientId(Guid.NewGuid().ToString()) 
                .WithTcpServer(_brokerAddress, _brokerPort) 
                .WithCleanSession()
                .Build();

            await _mqttClient.ConnectAsync(mqttOptions, stoppingToken);
            Console.WriteLine($"Connected to MQTT broker at {_brokerAddress}:{_brokerPort}");
        }

        _mqttClient.UseApplicationMessageReceivedHandler(e =>
        {
            var topic = e.ApplicationMessage.Topic;
            var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            Console.WriteLine($"Received MQTT message: Topic = {topic}, Message = {message}");
        });

        foreach (var topic in _topics)
        {
            {
                await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());
                Console.WriteLine($"Subscribed to topic: {topic}");
            }
        }
    }
                    
}




