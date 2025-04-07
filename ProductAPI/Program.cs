//using BuisinessLogicLayer;
//using BuisinessLogicLayer.Commands;
//using BuisinessLogicLayer.Handlers;
//using DataAccessLayer.Data;
//using DataAccessLayer.Repositories;
//using MediatR;
//using Microsoft.AspNetCore.Server.Kestrel;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Hosting;
//using MQTTnet;
//using MQTTnet.Client;
//using MQTTnet.Client.Options;
//using NServiceBus;
//using EndpointConfiguration = NServiceBus.EndpointConfiguration;

//namespace ProductAPI
//{
//    public class Program
//    {
//        public static async Task Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            var nServiceBusConfig = builder.Configuration.GetSection("NServiceBus");

//            var endpointConfiguration = new EndpointConfiguration("WebApi");
//            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
//            //transport.ConnectionString("host=localhost;username=guest;password=guest");

//            transport.ConnectionString(nServiceBusConfig.GetSection("Transport:ConnectionString").Value);
//            transport.UseConventionalRoutingTopology(QueueType.Quorum);

//            //endpointConfiguration.UseSerialization<SystemJsonSerializer>();  // JSON serializer (default in .NET)
//            var serializer = nServiceBusConfig.GetValue<string>("Serialization:Serializer");
//            if (serializer == "SystemJson")
//            {
//                endpointConfiguration.UseSerialization<SystemJsonSerializer>(); // JSON serializer (default in .NET)
//            }


//            endpointConfiguration.EnableInstallers();
//            var endpointInstance = await NServiceBus.Endpoint.Start(endpointConfiguration);
//            builder.Services.AddSingleton<IMessageSession>(endpointInstance);


//            builder.Services.AddMediatR(typeof(AddProductCommandHandler).Assembly);

//            builder.Services.AddScoped<IProductRepository, ProductRepository>();

//            // Add DbContext for database interaction
//            builder.Services.AddDbContext<ApplicationDbContext>(options =>
//                options.UseNpgsql(builder.Configuration.GetConnectionString("ProductConnection")));


//            // Register MQTT client as a singleton service
//            builder.Services.AddSingleton<IMqttClient>(provider =>
//            {
//                var factory = new MqttFactory();
//                var mqttClient = factory.CreateMqttClient();

//                // MQTT client options
//                var mqttOptions = new MqttClientOptionsBuilder()
//                    .WithClientId(Guid.NewGuid().ToString())
//                    .WithTcpServer("127.0.0.1", 1883)  // Replace with your MQTT broker
//                    .WithCleanSession()
//                    .Build();

//                mqttClient.ConnectAsync(mqttOptions).GetAwaiter().GetResult();  // Connect to the MQTT broker
//                return mqttClient;
//            });



//            // Register controllers and Swagger for API documentation
//            builder.Services.AddControllers();
//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen();

//            var app = builder.Build();

//            // Swagger configuration for API documentation
//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            // Set up the API middleware
//            app.UseHttpsRedirection();
//            app.UseAuthorization();
//            app.MapControllers();

//            // Run the application
//            app.Run();
//        }
//    }
//}


using BuisinessLogicLayer;
using BuisinessLogicLayer.Commands;
using BuisinessLogicLayer.Handlers;
using DataAccessLayer.Data;
using DataAccessLayer.Model;
using DataAccessLayer.Repositories;
using MediatR;
using Microsoft.AspNetCore.Server.Kestrel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using NServiceBus;
using EndpointConfiguration = NServiceBus.EndpointConfiguration;

namespace ProductAPI
{
    public class Program
    {
       
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            //builder.Services.Configure<MqttSettings>(builder.Configuration.GetSection("MqttSettings"));

            var nServiceBusConfig = builder.Configuration.GetSection("NServiceBus");

            var endpointConfiguration = new EndpointConfiguration("WebApi");
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.ConnectionString(nServiceBusConfig.GetSection("Transport:ConnectionString").Value);
            transport.UseConventionalRoutingTopology(QueueType.Quorum);


            var serializer = nServiceBusConfig.GetValue<string>("Serialization:Serializer");
            if (serializer == "SystemJson")
            {
                endpointConfiguration.UseSerialization<SystemJsonSerializer>();
            }

            endpointConfiguration.EnableInstallers();
            var endpointInstance = await NServiceBus.Endpoint.Start(endpointConfiguration);
            builder.Services.AddSingleton<IMessageSession>(endpointInstance);

            builder.Services.AddMediatR(typeof(AddProductCommandHandler).Assembly);

            builder.Services.AddScoped<IProductRepository, ProductRepository>();


            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("ProductConnection")));


            builder.Services.AddSingleton<IMqttClient>( provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var mqttsetting = configuration["Mqttsettings"];

                var brokerAddress = configuration.GetValue<string>("MqttSettings:BrokerAddress");
                var brokerPort = configuration.GetValue<int>("MqttSettings:BrokerPort");
                var clientId = configuration.GetValue<string>("MqttSettings:ClientId");
                var cleanSession = configuration.GetValue<bool>("MqttSettings:CleanSession");
                var keepAlivePeriod = configuration.GetValue<int>("MqttSettings:KeepAlivePeriod");

                var factory = new MqttFactory();
                var mqttClient = factory.CreateMqttClient();

                var mqttOptions = new MqttClientOptionsBuilder()
                    .WithClientId(clientId)
                    .WithTcpServer(brokerAddress, brokerPort)
                    .WithCleanSession(cleanSession)
                    .WithKeepAlivePeriod(TimeSpan.FromSeconds(keepAlivePeriod))
                    .Build();

                mqttClient.ConnectAsync(mqttOptions).GetAwaiter().GetResult();
                return mqttClient;
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}


