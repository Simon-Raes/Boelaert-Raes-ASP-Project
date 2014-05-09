using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomException
{
    public class MyBaseException : Exception
    {

        public MyBaseException()
        {
        }

        public MyBaseException(string message)
            : base(message)
        {
        }

        public MyBaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
