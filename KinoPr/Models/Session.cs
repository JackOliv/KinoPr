using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinoPr
{
    public class Session
    {
        public int id { get; set; }
        public DateTime time_start { get; set; }
        public DateTime time_end { get; set; }
        public string sessions { get; set; }
        public int session_status_id { get; set; }
        [JsonProperty("film")]
        public int FilmId { get; set; }
        public string FilmName { get; set; }
        public int hall { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }

        public class SessionResponse
        {
            public List<Session> Data { get; set; }
        }
    }

}
