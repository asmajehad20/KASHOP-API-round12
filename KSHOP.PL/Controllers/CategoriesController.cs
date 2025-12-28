using KSHOP.BLL.Service;
using KSHOP.DAL.Data;
using KSHOP.DAL.Dtos.Request;
using KSHOP.DAL.Dtos.Response;
using KSHOP.DAL.Models;
using KSHOP.DAL.Repository;
using KSHOP.PL.Resources;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace KSHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ICategoryService _categoryService;

        public CategoriesController(IStringLocalizer<SharedResource> localizer, ICategoryService categoryService) 
        { 
            _localizer = localizer;
            _categoryService = categoryService;
        }

        [HttpGet("")]
        public IActionResult Index([FromQuery] string lang = "en") 
        {
            var response =  _categoryService.GetAllCategoriesAsync("lang");
            return Ok(new {message = _localizer["Success"].Value , response });
        }

        [HttpPost("")]
        public IActionResult Create(CategoryRequest request)
        {
            var createdBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = _categoryService.CreateCategory(request);
            return Ok(new {message = _localizer["Success"].Value });
        }

    }
}
