using KSHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP.DAL.Repository
{
    public interface IReviewRepository
    {
        Task<bool> HasUserReviewdProduct(string userId, int productId);
        Task<Review> CreateAsync(Review request);
    }
}
