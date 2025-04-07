﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuisinessLogicLayer.Queries;
using DataAccessLayer.Model;
using DataAccessLayer.Repositories;
using MediatR;

namespace BuisinessLogicLayer.Handlers
{
    public class GetProductsQueryHandler : IRequestHandler<GetAllProductsQuery,List<Product> >
    {
        private readonly IProductRepository _productRepository;
        public GetProductsQueryHandler(IProductRepository productRepository) 
        {
            _productRepository = productRepository;
        }
        public async Task<List<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return await _productRepository.GetAllProducts();
        }
    }
}
