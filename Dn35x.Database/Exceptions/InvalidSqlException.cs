using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dn35x.Database.Exceptions
{
    public class InvalidSqlException : Exception
    {
        public InvalidSqlException(string message): base(message)
        {

        }
    }
}
