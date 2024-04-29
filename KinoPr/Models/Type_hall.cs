using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinoPr
{
    public class Type_hall
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public class TypeHallResponse
        {
            public List<Type_hall> Data { get; set; }
        }
    }
}
