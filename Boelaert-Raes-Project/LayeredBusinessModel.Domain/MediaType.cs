using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.Domain
{
    /**
     * Represents a MediaType
     */
    public class MediaType
    {
        public int media_id { get; set; }       //a unique id
        public String name { get; set; }   //description
    }
}
