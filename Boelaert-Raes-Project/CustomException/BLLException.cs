using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomException
{
    public class BLLException : MyBaseException
    {

        public BLLException()
        {
        }

        public BLLException(string message)
            : base(message)
        {
        }

        public BLLException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
