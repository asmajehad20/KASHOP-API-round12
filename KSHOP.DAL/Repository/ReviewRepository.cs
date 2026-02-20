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
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;
        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HasUserReviewdProduct(string userId, int productId)
        {
            return await _context.Reviews.AnyAsync(r=>r.UserId == userId && r.ProductId == productId);
        }

        public async Task<Review> CreateAsync(Review request)
        {
            await _context.AddAsync(request);
            await _context.SaveChangesAsync();
            return request;
        }
    }
}
