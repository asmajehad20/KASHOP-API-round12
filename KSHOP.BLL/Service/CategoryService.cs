using KSHOP.DAL.Data;
using KSHOP.DAL.Dtos.Response;
using KSHOP.DAL.Models;
using KSHOP.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using KSHOP.DAL.Dtos.Request;
namespace KSHOP.BLL.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository) 
        { 
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryResponse> CreateCategory(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
            
            await _categoryRepository.CreateAsync(category);
            return category.Adapt<CategoryResponse>();
        }

        public async Task<List<CategoryResponse>> GetAllCategoriesAsync(string lang = "en")
        {
            var categories = await _categoryRepository.GetAllAsync();

            foreach(var category in categories)
            {
                category.Translations = category.Translations.Where(t => t.Language == lang).ToList();
            }

            var response = categories.Adapt<List<CategoryResponse>>();
            return response;
        }

        public async Task<BaseResponse> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _categoryRepository.FindbyIdAsync(id);
                if (category == null) 
                {
                    return new BaseResponse()
                    {
                        Success = false,
                        Message = "category not found"
                    };

                }
                await _categoryRepository.DeleteAsync(category);
                
                return new BaseResponse()
                {
                    Success = true,
                    Message = "category deleted"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    Success = false,
                    Message = "cant delete category",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<BaseResponse> UpdateCategoryAsync(int id, CategoryRequest request)
        {
            try
            {
                var category = await _categoryRepository.FindbyIdAsync(id);
                if (category == null)
                {
                    return new BaseResponse()
                    {
                        Success = false,
                        Message = "category not found"
                    };

                }
                if (request.Translations != null)
                {
                    foreach (var translation in request.Translations)
                    {
                        var existing = category.Translations.FirstOrDefault(t => t.Language == translation.Language);

                        if (existing is not null)
                        {
                            existing.Name = translation.Name;
                        }
                        else
                        {
                            return new BaseResponse()
                            {
                                Success = false,
                                Message = "translation not supported"
                                
                            };
                        }
                    }
                }
                await _categoryRepository.UpdateAsync(category);
                return new BaseResponse()
                {
                    Success = true,
                    Message = "category updated successfully"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    Success = false,
                    Message = "cant update category",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<BaseResponse> ToggleStatus(int id)
        {
            try
            {
                var category = await _categoryRepository.FindbyIdAsync(id);
                if (category == null)
                {
                    return new BaseResponse()
                    {
                        Success = false,
                        Message = "category not found"
                    };

                }
                category.Status = category.Status == Status.Active ? Status.InActive :Status.Active;
                await _categoryRepository.UpdateAsync(category);
                return new BaseResponse()
                {
                    Success = false,
                    Message = "category updated successfully"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    Success = false,
                    Message = "cant update category",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

    }
}
