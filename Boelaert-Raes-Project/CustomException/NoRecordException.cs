using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomException
{
    public class NoRecordException : Exception
    {
        public NoRecordException()
        {
        }

        public NoRecordException(string message)
            : base(message)
        {
        }

        public NoRecordException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
