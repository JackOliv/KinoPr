using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinoPr
{
    public class Movie
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("duration")]
        public string Duration { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("director")]
        public string Director { get; set; }

        [JsonProperty("genreName")]
        public string GenreName { get; set; }
        [JsonProperty("genre_id")]
        public int GenreId { get; set; }

        [JsonProperty("photo")]
        public string Photo { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
    }
    public class MovieResponse
    {
        public List<Movie> Data { get; set; }
    }
}
