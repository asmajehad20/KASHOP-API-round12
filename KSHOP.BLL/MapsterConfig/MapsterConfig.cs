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
                .Select(t => t.Description).FirstOrDefault())
                .Map(dest => dest.SubImages, source => source.SubImages)
                .Map(dest => dest.Reviews, source => source.Reviews); 

            TypeAdapterConfig<Product, ProductResponse>.NewConfig()
                .Map(dest=>dest.MainImage, source => $"https://localhost:7082/images/{source.MainImage}")
                .Map(dest => dest.CreatedByUser, source => source.User.UserName)
                .Map(dest => dest.SubImages, source => source.SubImages);

            TypeAdapterConfig<Product, ProductUserResponse>.NewConfig()
                .Map(dest => dest.MainImage, source => $"https://localhost:7082/images/{source.MainImage}")
                .Map(dest => dest.Name, source => source.Translations
                .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                .Select(t=>t.Name).FirstOrDefault());

            TypeAdapterConfig<Order, OrderResponse>.NewConfig()
                .Map(dest => dest.UserName, source => source.User.UserName);

            //TypeAdapterConfig<Review, ReviewResponse>.NewConfig()
            //    .Map(dest => dest.UserName, source => source.User.UserName);

            TypeAdapterConfig<Review, ReviewResponse>.NewConfig()
               .Map(dest => dest.UserName, src => src.User != null ? src.User.UserName : null);
        }
    }
}
