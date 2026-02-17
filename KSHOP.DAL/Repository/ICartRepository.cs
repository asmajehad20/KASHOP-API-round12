using KSHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP.DAL.Repository
{
    public interface ICartRepository
    {
        Task<Cart> AddAsync(Cart request);
        Task<List<Cart>> GetUserCartAsync(string userId);
        Task<Cart> GetCartItemsAsync(string userId, int productId);
        Task<Cart> UpdateAsync(Cart cart);
    }
}
