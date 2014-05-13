using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LayeredBusinessModel.BLL
{
    public class Validation
    {
        /*
         * Checks if value of the String-object is numeric
         * Returns true if it is numeric, false of not
         */ 
        public Boolean isNumeric(String input)
        {
            return Regex.IsMatch(input, @"^\d+$");
        }
    }
}
