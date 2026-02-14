using KSHOP.BLL.Service;
using KSHOP.DAL.Dtos.Request;
using KSHOP.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KSHOP.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ProductsController(IProductService productService, IStringLocalizer<SharedResource> localizer)
        {
            _productService = productService;
            _localizer = localizer;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index([FromQuery] string lang = "en")
        {
            var response = await _productService.GetAllProductsForUserAsync();
            return Ok(new { message = _localizer["Success"].Value, response });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Index([FromRoute] int id, [FromQuery] string lang = "en")
        {
            var response = await _productService.GetProductDetailsAsync(id);
            return Ok(new { message = _localizer["Success"].Value, response });

        }
    }
    }
