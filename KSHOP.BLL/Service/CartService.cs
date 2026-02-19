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

            var cartItem = await _cartRepository.GetCartItemsAsync(userId, request.ProductId);
            var  existingCount = cartItem?.Count ?? 0;

            if (product.Quantity < (existingCount + request.Count))
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "not enough in stock"
                };
            }

            
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

        public async Task<BaseResponse> UpdateQuantityAsync(string userId, int productId, int count)
        {
            var cartItem = await _cartRepository.GetCartItemsAsync(userId, productId);
            var product = await _productRepository.FindByIdAsync(productId);

            //if(count <= 0)
            //{
            //    return new BaseResponse
            //    {
            //        Success = false,
            //        Message = "invalid count"
            //    };
            //}

            if(count == 0)
            {
                await _cartRepository.DeleteAsync(cartItem);
                return new BaseResponse
                {
                    Success = true,
                    Message = "product removed from cart"
                };
            }

            if(product.Quantity < count)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "not enough stock"
                };
            }

            cartItem.Count = count;
            await _cartRepository.UpdateAsync(cartItem);

            return new BaseResponse
            {
                Success = true,
                Message = "Quantity updated successfully"
            };
        }

        public async Task<BaseResponse> ClearCartAsync(string userId)
        {
            await _cartRepository.ClearCartAsync(userId);
            return new BaseResponse
            {
                Success = true,
                Message = "cart cleared successfully"
            };
        }

        public async Task<BaseResponse> RemoveFromCartAsync(string userId, int productId)
        {
            var cartItem = await _cartRepository.GetCartItemsAsync(userId, productId);
            if(cartItem is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "cart item is not found"
                };
            }

            await _cartRepository.DeleteAsync(cartItem);
            return new BaseResponse
            {
                Success = true,
                Message = "item removed successfully"
            };
        }
    }
}
