using KSHOP.DAL.Data;
using KSHOP.DAL.Dtos.Request;
using KSHOP.DAL.Dtos.Response;
using KSHOP.DAL.Models;
using KSHOP.DAL.Repository;
using Mapster;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP.BLL.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileService _fileService;


        public ProductService(IProductRepository productRepository, IFileService fileService) 
        {
            _productRepository = productRepository;
            _fileService = fileService;
        }

        public async Task<ProductResponse> CreateProduct(ProductRequest request)
        {
            var product = request.Adapt<Product>(); 
            
            if(request.MainImage != null)
            {
                var imagePath = await _fileService.UploadAsync(request.MainImage);
                product.MainImage = imagePath;
            }

            if (request.SubImages != null)
            {
                product.SubImages = new List<ProductImage>();
                foreach(var file in request.SubImages)
                {
                    var imagePath = await _fileService.UploadAsync(file);
                    product.SubImages.Add(new ProductImage { ImageName = imagePath });
                }
            }

            await _productRepository.AddAsync(product);
            var respose = product.Adapt<ProductResponse>();
            respose.SubImages = product.SubImages.Select(s => s.ImageName).ToList();
            return respose;
        }

        public async Task<List<ProductResponse>> GetAllProductsForAdminAsync()
        {
            var products = await _productRepository.GetAllAsync();
            var response = products.Adapt<List<ProductResponse>>();
            return response;
        }

        public async Task<List<ProductUserResponse>> GetAllProductsForUserAsync(string lang = "en", int page = 1, int limit = 3, string? search = null)
        {
            var query = _productRepository.Query();

            if(search is not null)
            {
                query = query.Where(p=>p.Translations.Any(t=>t.Language == lang && t.Name.Contains(search) || t.Description.Contains(search)));
            }

            var totalCount = await query.CountAsync();

            query = query.Skip((page - 1) * limit).Take(limit);

            var response = query.BuildAdapter().AddParameters("lang", lang).AdaptToType<List<ProductUserResponse>>();
            return response;
        }

        public async Task<ProductUserDetails> GetProductDetailsAsync(int id, string lang = "en")
        {
            var product = await _productRepository.FindByIdAsync(id);
            var response = product.BuildAdapter().AddParameters("lang", lang).AdaptToType<ProductUserDetails>();
            return response;
        }
    }
}
