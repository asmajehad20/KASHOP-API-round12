using KSHOP.DAL.Data;
using KSHOP.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP.DAL.Repository
{
    public class OrderRepository :IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateAsync(Order request)
        {
            await _context.AddAsync(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task<Order> GetBySessionIdAsync(string sessionId)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.SessionId == sessionId);
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }
    }
}
