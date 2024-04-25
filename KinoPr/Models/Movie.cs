﻿using Newtonsoft.Json;
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

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("duration")]
        public string Duration { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("director")]
        public string Director { get; set; }

        [JsonProperty("genre_id")]
        public int GenreId { get; set; }

        [JsonProperty("photo")]
        public string Photo { get; set; }
    }
    public class MovieResponse
    {
        public List<Movie> Data { get; set; }
    }
}
