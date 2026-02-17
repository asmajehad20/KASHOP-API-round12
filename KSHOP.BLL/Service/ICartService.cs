using KSHOP.DAL.Dtos.Request;
using KSHOP.DAL.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP.BLL.Service
{
    public interface ICartService
    {
        Task<BaseResponse> AddToCartAsync(string userId, AddToCartRequest request);
        Task<CartSummaryResponse> GetUserCartAsync(string userId, string lang = "en");
    }
}
