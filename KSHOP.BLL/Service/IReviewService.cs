using KSHOP.DAL.Dtos.Request;
using KSHOP.DAL.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP.BLL.Service
{
    public interface IReviewService
    {
        Task<BaseResponse> AddReviewAsync(string userId, int productId, CreateReviewRequest request);
    }
}
