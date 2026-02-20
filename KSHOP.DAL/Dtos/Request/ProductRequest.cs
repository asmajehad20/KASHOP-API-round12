using KSHOP.DAL.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP.DAL.Dtos.Request
{
    public class ProductRequest
    {
        public List<ProductTranslationRequest>? Translations { get; set; }
        public decimal Price { get; set; }

        [MinValue]
        public decimal? Discount { get; set; }
        public int Quantity { get; set; }
        
        public IFormFile? MainImage { get; set; }
        public List<IFormFile>? SubImages { get; set; }
        public int CategoryId { get; set; }
    }
}
