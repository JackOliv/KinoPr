using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinoPr
{
    public class Film
    {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Duration { get; set; }
            public int GenreId { get; set; }
            public string Photo { get; set; }
            public int Year { get; set; }
            public string Description { get; set; }
            public string Director { get; set; }
            public string Country { get; set; }
        
    }
}
