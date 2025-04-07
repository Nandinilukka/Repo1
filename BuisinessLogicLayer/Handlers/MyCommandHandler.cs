using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuisinessLogicLayer.RabbitMqCommands;
using DataAccessLayer.Model;

namespace BuisinessLogicLayer.Handlers
{
    public class MyCommandHandler : IHandleMessages<CreateProductCommand>,IHandleMessages<DeleteProductsCommand>
    {

        public  Task Handle(CreateProductCommand message, IMessageHandlerContext context)
        {
            Console.WriteLine($"Received CreateProductCommand: ProductName = {message.ProductName}, Quantity = {message.Quantity}");
            return Task.CompletedTask;
        }

        public Task Handle(DeleteProductsCommand message, IMessageHandlerContext context)
        {
            Console.WriteLine("Recieved deleteProductmessage");
            return Task.CompletedTask;
        }



        //public async Task Handle(CreateProductCommand message, IMessageHandlerContext context)
        //{
        //    Console.WriteLine($"Received CreateProductCommand: ProductName = {message.ProductName}, Quantity = {message.Quantity}");
        //    var newProduct = new Product
        //    {
        //        productName = message.ProductName,
        //        quantity = message.Quantity,
        //    };
        //    await _productRepository.AddProduct(newProduct);
        //    Console.WriteLine($"Product added: {message.ProductName}, Quantity: {message.Quantity}");
        }
    }

