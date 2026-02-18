using KSHOP.DAL.Dtos.Response;
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
        Task<bool> DecreaseQuantitesAsync(List<(int productId, int quantity)> items);
        Task<Product> FindByIdAsync(int id);
        public IQueryable<Product> Query();
    }
}
