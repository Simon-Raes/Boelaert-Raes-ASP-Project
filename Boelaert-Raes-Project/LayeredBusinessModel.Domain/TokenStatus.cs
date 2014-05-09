using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayeredBusinessModel.Domain
{
    public enum TokenStatus
    {
        EMPTY,          //nothing
        VERIFICATION,   //id = 1
        RECOVERY        //id = 2
    }
}
