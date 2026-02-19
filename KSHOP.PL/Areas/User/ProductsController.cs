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
        public async Task<IActionResult> Index(
            [FromQuery] string lang = "en",
            [FromQuery] int page = 1, 
            [FromQuery] int limit = 3,
            [FromQuery] string? search= null,
            [FromQuery] int? categoryId = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool asc = true
            )
        {
            var response = await _productService.GetAllProductsForUserAsync(lang, page, limit, search, categoryId, minPrice, maxPrice, sortBy, asc);
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
