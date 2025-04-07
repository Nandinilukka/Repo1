using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuisinessLogicLayer.Commands;
using DataAccessLayer.Model;
using DataAccessLayer.Repositories;
using MediatR;

namespace BuisinessLogicLayer.Handlers
{
    public class AddProductCommandHandler : IRequestHandler<AddproductCommand, Product>
    {
        private readonly IProductRepository _productRepository;
        public AddProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<Product> Handle(AddproductCommand request, CancellationToken cancellationToken)
        {
            var newProduct = new Product
            {
               productName=request.productName,
               quantity=request.Quantity,
            };
            return await _productRepository.AddProduct(newProduct);
            
        }
    }
}
