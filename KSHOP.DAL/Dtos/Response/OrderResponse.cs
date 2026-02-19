using KSHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KSHOP.DAL.Dtos.Response
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatusEnum PaymentStatus { get; set; }
        public decimal AmountPaid { get; set; }
        public string UserName { get; set; }
    }
}
