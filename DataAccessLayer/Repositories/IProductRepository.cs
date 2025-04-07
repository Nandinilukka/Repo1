using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Model;

namespace DataAccessLayer.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task<Product> AddProduct(Product product);
        Task<bool> DeleteProductById(int id);
        
    }
}
