using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers_support_chat_bot.Exceptions
{
    [Serializable]
    public class DBException : Exception
    {
        public DBException() : base() { }
        public DBException(string message) : base(message) { }
        public DBException(string message, Exception inner) : base(message, inner) { }
    }
}