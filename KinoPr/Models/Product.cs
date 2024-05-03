using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinoPr
{
    public class Product
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("mass")]
        public int Mass { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
        [JsonProperty("photo")]
        public string Photo { get; set; }
        public class ProductResponse
        {
            public List<Product> Data { get; set; }
        }
    }
}
