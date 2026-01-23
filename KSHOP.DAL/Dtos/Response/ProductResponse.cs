using KSHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KSHOP.DAL.Dtos.Response
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public String CreatedByUser { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; set; }
        public string MainImage { get; set; }
        public List<string> SubImages { get; set; }
        public List<CategoryTranslationResponse> Translations { get; set; }
    }
}
