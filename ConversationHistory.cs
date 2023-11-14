using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers_support_chat_bot
{
    internal class ConversationHistory
    {
        private List<String>? _userMessages { get; set; }
        private List<String>? _botMessages { get; set; }

        public ConversationHistory() 
        { 
            this._userMessages = new List<String>();
            this._botMessages = new List<String>();
        }

        public String? AddUserMessage(String message)
        {
            try {
                if (this._userMessages == null) throw new ArgumentNullException(nameof(this._userMessages));
                else this._userMessages.Add(message);
            }
            catch (Exception ex) 
            { 
                return ex.Message;
            }

            return null;
        }

        public String? AddBotMessage(String message)
        {
            try
            {
                if (this._botMessages == null) throw new ArgumentNullException(nameof(this._botMessages));
                else this._botMessages.Add(message);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return null;
        }
    }
}
