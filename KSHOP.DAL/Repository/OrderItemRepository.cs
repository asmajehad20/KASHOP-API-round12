using KSHOP.DAL.Data;
using KSHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP.DAL.Repository
{
    public class OrderItemRepository :IOrderItemRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(List<OrderItem> orderItems)
        {
            await _context.AddRangeAsync(orderItems);
            await _context.SaveChangesAsync();
            
        }
    }
}
