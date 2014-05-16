using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.Domain
{
    /**
     * Represents a DVD
     */
    public class DvdInfo
    {
        public String dvd_info_id { get; set; }        //a unique id
        public String name { get; set; }            //name
        public String year { get; set; }            //year
        public String barcode { get; set; }         //barcode
        public String author { get; set; }          //author
        
        public String descripion { get; set; }      //description
        public float rent_price { get; set; }       //rent price
        public float buy_price { get; set; }        //but price
        public DateTime date_added { get; set; }    //date this dvd was added
        public int amount_sold { get; set; }        //The amount sold for this dvd


        public List<KeyValuePair<int, string>> media { get; set; }  //Media linked with this dvd
        public String[] actors { get; set; }        //List of actors
        public String duration { get; set; }        //Duration
        public List<Genre> genres { get; set; }     //list of gengres
    }
}
