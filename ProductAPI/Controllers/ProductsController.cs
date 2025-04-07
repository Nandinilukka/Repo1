using BuisinessLogicLayer.Commands;
using BuisinessLogicLayer.Queries;
using BuisinessLogicLayer.RabbitMqCommands;
using DataAccessLayer.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MQTTnet;
using MQTTnet.Client;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMessageSession _messageSession;
        private readonly IMqttClient _mqttClient;

        public ProductsController(IMediator mediator, IMessageSession messageSession,IMqttClient mqttClient)
        {
            _mediator = mediator;
            _messageSession = messageSession;
            _mqttClient = mqttClient;
        }

        [HttpGet]
        public async Task<ActionResult<Product>> GetProducts()
        {
            var product = await _mediator.Send(new GetAllProductsQuery());
            if (product == null || product.Count == 0)
            {
                return BadRequest("products not found");
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct([FromBody]CreateProductCommand command,[FromQuery]AddproductCommand add)
        {
            var product = await _mediator.Send(add);
            var cmd = new CreateProductCommand
            {
                ProductName=command.ProductName,
                Quantity=command.Quantity,
            };
            var mqttMessage = new MqttApplicationMessageBuilder()
                .WithTopic("product/added")  // Topic name
                .WithPayload($"Product Added: {command.ProductName}, Quantity: {command.Quantity}")
                .WithAtLeastOnceQoS()  
                .Build();

            await _mqttClient.PublishAsync(mqttMessage);  
            await _messageSession.Publish(cmd);           
            if (cmd != null)
            {
                return Ok("Data is added");
            }
            return NotFound("Product Not added");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var command = new GetProductByIdQuery { ProductId = id };
            var result = await _mediator.Send(command);
            if (result != null)
            {
                var mqttMessage = new MqttApplicationMessageBuilder()
                    .WithTopic("product/fetched")  // Topic name
                    .WithPayload($"Product Fetched: {result.productName}, Quantity: {result.quantity}")
                    .WithAtLeastOnceQoS()  
                    .Build();

                await _mqttClient.PublishAsync(mqttMessage);
                return Ok(result);
            }
            return NotFound("Id not found");
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var command = new DeleteProductsCommand { ProductId = id };
            await _messageSession.SendLocal(command);

            //var product=await _mediator.Send(command);

            if (command != null)
            {
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic("product/deleted")
                    .WithPayload($"Product ID {id} is deleted")
                    .WithExactlyOnceQoS()
                    .Build();

                await _mqttClient.PublishAsync(message);
                return Ok("product is Deleted ");
            }
            return NotFound("Id not found");

        }
    }
}
