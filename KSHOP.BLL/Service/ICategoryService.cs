using KSHOP.DAL.Dtos.Request;
using KSHOP.DAL.Dtos.Response;
using KSHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP.BLL.Service
{
    public interface ICategoryService
    {
        List<CategoryResponse> GetAllCategories();
        CategoryResponse CreateCategory(CategoryRequest request);
    }
}
