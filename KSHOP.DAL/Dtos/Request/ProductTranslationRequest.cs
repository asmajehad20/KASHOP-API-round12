using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP.DAL.Dtos.Request
{
    public class ProductTranslationRequest
    {
        public String? Name { get; set; }
        public String? Description { get; set; }
        public String Language { get; set; } = "en";
    }
}
