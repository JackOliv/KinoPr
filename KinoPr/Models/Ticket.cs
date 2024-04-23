using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinoPr
{
    public class Ticket
    {
        public int Id { get; set; }
        public DateTime SessionTime { get; set; }
        public decimal Price { get; set; }
        public string SeatNumber { get; set; }
        public int UserId { get; set; }
        public int SessionId { get; set; }
    }
}
