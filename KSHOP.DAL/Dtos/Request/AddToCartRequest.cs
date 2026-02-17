using KSHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP.DAL.Dtos.Request
{
    public class AddToCartRequest
    {
        public int ProductId { get; set; }
        public int Count { get; set; } = 1;
    }
}
