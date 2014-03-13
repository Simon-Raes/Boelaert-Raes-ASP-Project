using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.Domain
{
    public class Genre
    {
        public int genre_id { get; set; }
        public int category_id { get; set; }
        public String name { get; set; }
    }
}
