using KSHOP.DAL.Dtos.Request;
using KSHOP.DAL.Dtos.Response;
using KSHOP.DAL.Models;
using KSHOP.DAL.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP.BLL.Service
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IProductRepository _productRepository;


        public CheckoutService(ICartRepository cartRepository, IOrderRepository orderRepository,
            UserManager<ApplicationUser> userManager, IEmailSender emailSender,
            IOrderItemRepository orderItemRepository, IProductRepository productRepository) 
        { 
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
            _emailSender = emailSender;
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
        
        }

        public async Task<CheckoutResponse> ProcessPaymentAsync(CheckoutRequest request, string userId)
        {
            var cartItems = await _cartRepository.GetUserCartAsync(userId);
            if (!cartItems.Any())
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Message = "cart is empty"
                };
            }

            decimal totalAmount = 0;

            foreach(var cart in cartItems)
            {
                if(cart.Product.Quantity < cart.Count)
                {
                    return new CheckoutResponse
                    {
                        Success = false,
                        Message = "not enough in stock"
                    };
                }
                totalAmount += cart.Product.Price * cart.Count;
            }

            Order order = new Order
            {
                UserId = userId,
                PaymentMethod = request.PaymentMethod,
                AmountPaid = totalAmount,
                PaymentStatus = PaymentStatusEnum.UnPaid
            };

            if (request.PaymentMethod == PaymentMethodEnum.Cash) 
            {
                return new CheckoutResponse
                {
                    Success = true,
                    Message = "cash"
                };
            }

            if(request.PaymentMethod == PaymentMethodEnum.Visa) 
            {
                var options = new SessionCreateOptions

                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = $"https://localhost:7082/api/checkouts/Success?session_id={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = $"https://localhost:7082/api/checkouts/cancel",
                    Metadata = new Dictionary<string, string>
                    {
                        {"UserId", userId },
                    },
                };

                foreach (var item in cartItems)
                {
                    options.LineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "USD",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Translations.FirstOrDefault(t => t.Language == "en")?.Name ?? "Product"
                            },
                            UnitAmount = (long)(item.Product.Price * 100), 
                        },
                        Quantity = item.Count,
                    });
                }

                var service = new SessionService();
                var session = service.Create(options);
                order.SessionId = session.Id;
                order.PaymentStatus = PaymentStatusEnum.Paid;
                

                await _orderRepository.CreateAsync(order);

                return new CheckoutResponse
                {
                    Success = true,
                    Message = "payment session created",
                    Url = session.Url
                };
            }

            else
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Message = "invalid payment method"
                };
            }

        }


        public async Task<CheckoutResponse> HandleSuccessAsync(string sessionId)
        {
            var service = new SessionService();
            var session = service.Get(sessionId);
            var userId = session.Metadata["UserId"];
            //Console.WriteLine(userId);

            var order = await _orderRepository.GetBySessionIdAsync(sessionId);
            order.PaymentId = session.PaymentIntentId;
            order.OrderStatus = OrderStatusEnum.Approved;
            await _orderRepository.UpdateAsync(order);

            var user = await _userManager.FindByEmailAsync(userId);
            
            var carItems = await _cartRepository.GetUserCartAsync(userId);
            
            var orderItems = new List<OrderItem>();

            var productUpdated = new List<(int productId, int quantity)>();

            foreach(var cartItem in carItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    UnitPrice = cartItem.Product.Price,
                    Quantity = cartItem.Count,
                    TotalPrice = cartItem.Product.Price * cartItem.Count,
                };
                orderItems.Add(orderItem);
                productUpdated.Add((cartItem.ProductId, cartItem.Count));
                
            }

            await _orderItemRepository.CreateAsync(orderItems);
            await _cartRepository.ClearCartAsync(userId);
            await _productRepository.DecreaseQuantitesAsync(productUpdated);
            await _emailSender.SendEmailAsync(user.Email, "payment successfuly", "<h2>thank you ...</h2>");
            
            return new CheckoutResponse
            {
                Success = true,
                Message = "payment completed successfully",
            };
        }
    }
}
