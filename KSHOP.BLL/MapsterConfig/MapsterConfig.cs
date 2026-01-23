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
                .Map(dest => dest.CreatedByUser, source => source.User.UserName);

            TypeAdapterConfig<Product, ProductResponse>.NewConfig()
                .Map(dest=>dest.MainImage, source => $"https://localhost:7082/images/{source.MainImage}");
        }
    }
}
