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
    public class CartService : ICartService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;

        public CartService(IProductRepository productRepository, ICartRepository cartRepository)
        {
            _productRepository = productRepository;
            _cartRepository = cartRepository;
        }

        public async Task<BaseResponse> AddToCartAsync(string userId, AddToCartRequest request)
        {
            var product = await _productRepository.FindByIdAsync(request.ProductId);
            if (product == null) 
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "product not found"
                };
            }
            if (product.Quantity < request.Count)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "not enough in stock"
                };
            }

            var cartItem = await _cartRepository.GetCartItemsAsync(userId, request.ProductId);
            if (cartItem is not null)
            {
                cartItem.Count += request.Count;
                await _cartRepository.UpdateAsync(cartItem);
            }
            else
            {
                var cart = request.Adapt<Cart>();
                cart.UserId = userId;

                await _cartRepository.AddAsync(cart);

            }
                
            return new BaseResponse
            {
                Success = true,
                Message = "product added to cart successfully"
            };
        }
    
        public async Task<CartSummaryResponse> GetUserCartAsync(string userId, string lang = "en")
        {
            var cartItem = await _cartRepository.GetUserCartAsync(userId);

            var items = cartItem.Select(c => new CartResponse
            {
                ProductId = c.ProductId,
                ProductName = c.Product.Translations.FirstOrDefault(t => t.Language == lang).Name,
                Count = c.Count,
                Price = c.Product.Price,

            }).ToList();
            return new CartSummaryResponse
            {
                Items = items,
            };
        }
    }
}
