using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class Product
    {
        public int productId { get; set; } 
        public string productName { get; set; }
        public int quantity { get; set; }
    }
}
