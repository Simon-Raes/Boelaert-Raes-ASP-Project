using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.Domain
{
    /*
     *  Represents a genre
     */
    public class Genre
    {
        public int genre_id { get; set; }       //a unique id
        public Category category { get; set; }  //the category
        public String name { get; set; }        //description
    }
}
