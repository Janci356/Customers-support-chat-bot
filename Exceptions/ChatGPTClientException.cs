using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers_support_chat_bot.Exceptions
{
    [Serializable]
    public class ChatGPTClientException : Exception
    {
        public ChatGPTClientException() : base() { }
        public ChatGPTClientException(string message) : base(message) { }
        public ChatGPTClientException(string message, Exception inner) : base(message, inner) { }
    }
}