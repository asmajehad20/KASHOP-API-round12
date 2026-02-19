using KSHOP.DAL.Dtos.Request;
using KSHOP.DAL.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP.BLL.Service
{
    public interface IProductService
    {
        Task<ProductResponse> CreateProduct(ProductRequest request);
        Task<List<ProductResponse>> GetAllProductsForAdminAsync();

        Task<PaginatedResponse<ProductUserResponse>> GetAllProductsForUserAsync(string lang = "en",
            int page = 1,
            int limit = 3,
            string? search = null,
            int? categoryId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string? sortBy = null,
            bool asc = true);

        Task<ProductUserDetails> GetProductDetailsAsync(int id, string lang = "en");
    }
}
