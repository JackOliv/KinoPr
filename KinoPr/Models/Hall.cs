using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinoPr
{
    public class Hall
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("row_quantity")]
        public int RowQuantity { get; set; }
        [JsonProperty("place_quantity")]
        public int PlaceQuantity { get; set; }
        [JsonProperty("type_hall_id")]
        public int TypeHallId { get; set; }
        [JsonProperty("price")]
        public int Price { get; set; }
        public class HallResponse
        {
            public List<Hall> Data { get; set; }
        }
    }
}
