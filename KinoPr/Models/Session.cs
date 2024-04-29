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
        public int Id { get; set; }
        public DateTime time_start { get; set; }
        public DateTime time_end { get; set; }
        public string sessions { get; set; }
        public int session_status_id { get; set; }
        public string film { get; set; }
        public int film_id { get; set; }
        public string type_hall { get; set; }
        public int type_hall_id { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }

        public class SessionResponse
        {
            public List<Session> Data { get; set; }
        }
    }
}
