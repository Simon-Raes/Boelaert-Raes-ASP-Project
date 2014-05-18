using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.Domain
{
    /**
     * Represents a Category
     */  
    public class Category
    {
        public int category_id { get; set; }    //a unique id
        public String name { get; set; }        //a description
    }
}
