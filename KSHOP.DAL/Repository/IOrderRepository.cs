using KSHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP.DAL.Repository
{
    public interface IOrderRepository
    {
        Task<Order> CreateAsync(Order request);
        Task<Order> GetBySessionIdAsync(string sessionId);
        Task<Order> UpdateAsync(Order order);
        Task<List<Order>> GetOrdersByStatusAsync(OrderStatusEnum status);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<bool> HasUserDeliverdOrderForProduct(string userId, int productId);
    }
}
