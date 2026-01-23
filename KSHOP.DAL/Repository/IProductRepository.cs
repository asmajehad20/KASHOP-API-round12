using KSHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP.DAL.Repository
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product request);
        Task<List<Product>> GetAllAsync();
    }
}
