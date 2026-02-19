using KSHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP.DAL.Dtos.Request
{
    public class UpdateOrderStatusRequest
    {
        public OrderStatusEnum Status { get; set; }
    }
}
