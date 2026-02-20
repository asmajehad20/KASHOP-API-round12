using KSHOP.DAL.Dtos.Request;
using KSHOP.DAL.Dtos.Response;
using KSHOP.DAL.Models;
using KSHOP.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP.BLL.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IOrderRepository orderRepository, IReviewRepository reviewRepository)
        {
            _orderRepository = orderRepository;
            _reviewRepository = reviewRepository;
        }

        public async Task<BaseResponse> AddReviewAsync(string userId, int productId, CreateReviewRequest request)
        {
            var hasDeliverdOrder = await _orderRepository.HasUserDeliverdOrderForProduct(userId, productId);

            if (!hasDeliverdOrder)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "you can only review a product once u receive it"
                };
            }

            var alreadyReview = await _reviewRepository.HasUserReviewdProduct(userId, productId);

            if (alreadyReview)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "cant add another review"
                };
            }

            var review = request.Adapt<Review>();
            review.UserId = userId;
            review.ProductId = productId;

            await _reviewRepository.CreateAsync(review);

            return new BaseResponse
            {
                Success = true,
                Message = "review added successfully"
            };
        }
    }
}
