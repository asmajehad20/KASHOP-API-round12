using KSHOP.DAL.Dtos.Response;
using KSHOP.DAL.Models;
using Mapster;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP.BLL.MapsterConfig
{
    public static class MapsterConfig
    {
        public static void MapsterConfigRegister()
        {
            TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
                .Map(dest => dest.CreatedByUser, source => source.CreatedBy);

            TypeAdapterConfig<Product, ProductUserDetails>.NewConfig()
                .Map(dest => dest.Name, source => source.Translations.
                Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                .Select(t => t.Name).FirstOrDefault())
                .Map(dest => dest.Description, source => source.Translations.
                Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                .Select(t => t.Name).FirstOrDefault());

            TypeAdapterConfig<Product, ProductResponse>.NewConfig()
                .Map(dest=>dest.MainImage, source => $"https://localhost:7082/images/{source.MainImage}");

            TypeAdapterConfig<Product, ProductUserResponse>.NewConfig()
                .Map(dest => dest.MainImage, source => $"https://localhost:7082/images/{source.MainImage}")
                .Map(dest => dest.Name, source => source.Translations
                .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                .Select(t=>t.Name).FirstOrDefault());

            
        }
    }
}
