﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.Domain
{
    /*
     * Represents PageVisits
     */ 
    public class PageVisits
    {
        public Customer customer { get; set; }
        public DvdInfo dvdInfo { get; set; }
        public int number_of_visits { get; set; }   //the amount the customer has visited this dvd
    }
}
