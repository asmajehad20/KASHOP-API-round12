using KSHOP.BLL.Service;
using KSHOP.DAL.Dtos.Request;
using KSHOP.DAL.Models;
using KSHOP.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KSHOP.PL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public OrdersController(IOrderService order, IStringLocalizer<SharedResource> localizer)
        {
            _orderService = order;
            _localizer = localizer;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetOrders([FromQuery]OrderStatusEnum status = OrderStatusEnum.Pending)
        {
            var orders = await _orderService.GetOrdersAsync(status);
            return Ok(orders);
        }

        [HttpPatch("{orderId}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] int orderId, [FromBody] UpdateOrderStatusRequest request)
        {
            var result = await _orderService.UpdateOrderStatusAsync(orderId, request.Status);
            if(!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}
