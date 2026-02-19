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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }

        public async Task<List<OrderResponse>> GetOrdersAsync(OrderStatusEnum status)
        {
            var orders = await _orderRepository.GetOrdersByStatusAsync(status);
            var result = orders.Adapt<List<OrderResponse>>();
            return result;
        }

        public async Task<BaseResponse> UpdateOrderStatusAsync(int orderId, OrderStatusEnum newStatus)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            order.OrderStatus = newStatus;

            if(newStatus == OrderStatusEnum.Delivered)
            {
                order.PaymentStatus = PaymentStatusEnum.Paid;
            }
            //else if(newStatus == OrderStatusEnum.Cancelled)
            //{
            //    if(order.OrderStatus == OrderStatusEnum.Shipped)
            //    {
            //        return new BaseResponse
            //        {
            //            Success = false,
            //            Message = "cant cancel this order"
            //        };
            //    }
            //}
            await _orderRepository.UpdateAsync(order);
            return new BaseResponse
            {
                Success = true,
                Message = "order status updated"
            };
        }

    }
}
