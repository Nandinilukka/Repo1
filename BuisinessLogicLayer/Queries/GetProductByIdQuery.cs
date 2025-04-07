using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Model;
using MediatR;

namespace BuisinessLogicLayer.Queries
{
    public class GetProductByIdQuery:IRequest<Product>
    {
        public int ProductId { get; set; }
    }
}
