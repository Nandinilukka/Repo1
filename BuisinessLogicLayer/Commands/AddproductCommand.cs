using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Model;
using MediatR;

namespace BuisinessLogicLayer.Commands
{
    public class AddproductCommand:IRequest<Product>
    {
        public string productName {  get; set; }
        public int Quantity {  get; set; }

    }
}
