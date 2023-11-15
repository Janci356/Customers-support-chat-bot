using Customers_support_chat_bot.Core;
using Customers_support_chat_bot.MVVM.Model;
using System;
using System.Collections.ObjectModel;
namespace Customers_support_chat_bot.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {

        public ObservableCollection<MessageModel> Messages { get; set; }

        public RelayCommand SendCommand { get; set; }

        private string _message;

        public string Message
        {
            get { return _message; }
            set 
            {
                _message = value;
                OnPropertyChanged();
            }
        }


        public MainViewModel()
        {
            Messages = new ObservableCollection<MessageModel>();

            SendCommand = new RelayCommand(o =>
            {
                if(Message != "" && Message != null)
                {
                    Messages.Add(new MessageModel
                    {
                        Username = "Tomas",
                        ImageSource = "./Icons/user.png",
                        Message = Message,
                        Time = DateTime.Now,
                        IsNativeOrigin = true,
                        FirstMessage = true
                    });

                    Message = "";
                }
            });

            Messages.Add(new MessageModel
            {
                Username = "Tomas",
                ImageSource = "./Icons/user.png",
                Message = "Test message",
                Time = DateTime.Now,
                IsNativeOrigin = false,
                FirstMessage = true,
            });

            for (int i = 0; i < 3; i++)
            {
                Messages.Add(new MessageModel
                {
                    Username = "Tomas",
                    ImageSource = "./Icons/user.png",
                    Message = "Test message",
                    Time = DateTime.Now,    
                    IsNativeOrigin = false,
                    FirstMessage = true,
                });
            }
        }
    }
}
