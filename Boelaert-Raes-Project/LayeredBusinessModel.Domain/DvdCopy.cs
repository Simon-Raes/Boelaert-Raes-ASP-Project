using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.Domain
{
    /*
     * Represents a DVDcopy
     */ 
    public class DvdCopy
    {
        public int dvd_copy_id { get; set; }        //Unique copy number
        
        
        //public int dvd_info_id { get; set; }        //ID from dvdInfo
        public DvdInfo dvdinfo { get; set; }        //dvd info

        //public int copy_type_id { get; set; }       //type indicating this copy is for rent or for sale
        public DvdCopyType type { get; set; }
        
        public String serialnumber { get; set; }    //serialnumber
        public String note { get; set; }            //a brief note
        public Boolean in_stock { get; set; }        //flag indicating this copy is still in stock or not
    }
}
