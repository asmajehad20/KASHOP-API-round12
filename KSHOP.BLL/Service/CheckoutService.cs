using KSHOP.DAL.Dtos.Request;
using KSHOP.DAL.Dtos.Response;
using KSHOP.DAL.Repository;
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

        public CheckoutService(ICartRepository cartRepository) 
        { 
            _cartRepository = cartRepository;
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

            if(request.PaymentMethod == "cash") 
            {
                return new CheckoutResponse
                {
                    Success = true,
                    Message = "cash"
                };
            }

            if(request.PaymentMethod == "visa") 
            {
                var options = new SessionCreateOptions

                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = $"https://localhost:7082/api/checkout/success",
                    CancelUrl = $"https://localhost:7082/api/checkout/cancel",
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
                            UnitAmount = (long)(item.Product.Price * 100), // convert to cents
                        },
                        Quantity = item.Count,
                    });
                }

                var service = new SessionService();
                var session = service.Create(options);

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
    }
}
