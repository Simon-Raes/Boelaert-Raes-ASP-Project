﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.Domain
{
    public class Order
    {
        public int order_id { get; set; }
        public int customer_id { get; set; }
        public int orderstatus_id { get; set; }
        public DateTime date { get; set; }
    }
}