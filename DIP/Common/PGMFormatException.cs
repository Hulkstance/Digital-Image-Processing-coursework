using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIP.Common
{
    public class PGMFormatException : Exception
    {
        public PGMFormatException(string message) 
            : base(message)
        {
        }
    }
}
