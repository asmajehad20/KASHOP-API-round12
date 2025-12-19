using KSHOP.BLL.Service;
using KSHOP.DAL.Dtos.Request;
using KSHOP.PL.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KSHOP.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _category;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CategoriesController(ICategoryService category, IStringLocalizer<SharedResource> localizer)
        {
            _category = category;
            _localizer = localizer;
        }

        [HttpGet("")]
        public IActionResult Index() 
        {
            var response = _category.GetAllCategories();
            return Ok(new {message = _localizer["Success"].Value, response });
        }

        
    }
}
