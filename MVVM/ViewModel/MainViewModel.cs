using Customers_support_chat_bot.Core;
using Customers_support_chat_bot.Exceptions;
using Customers_support_chat_bot.MVVM.Model;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using Customers_support_chat_bot.enums;
using Customers_support_chat_bot.Models;

namespace Customers_support_chat_bot.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {

        public ObservableCollection<MessageModel> Messages { get; set; }

        private UserDbContext DbContext { get; set; }

        private ChatGPTClient Client {  get; set; } 

        private StreamWriter ChatLogFile {  get; set; }
    

        /* Commands*/
        public RelayCommand SendCommand { get; set; }

        public RelayCommand LoginCommand { get; set; }

        public RelayCommand CloseCommand { get; set; }

        public RelayCommand SwitchToSignupCommand { get; set; } 

        public RelayCommand SwitchToLoginCommand { get; set; }  

        public RelayCommand SignupCommand { get; set; } 


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

        private bool _messagesEnabled;

        public bool MessagesEnabled
        {
            get { return _messagesEnabled; }
            set { 
                _messagesEnabled = value;
                OnPropertyChanged("");
            }
        }

        private Visibility _loginVisibility;

        public Visibility LoginVisibility
        {
            get { return _loginVisibility; }
            set {
                _loginVisibility = value;
                OnPropertyChanged("LoginVisibility");    
            }
        }

        private Visibility _signupVisibility;

        public Visibility SignupVisibility
        {
            get { return _signupVisibility; }
            set { 
                _signupVisibility = value;
                OnPropertyChanged("SignupVisibility");
            }
        }

        private Visibility _loggedIn;

        public Visibility LoggedIn
        {
            get { return _loggedIn; }
            set {
                _loggedIn = value;
                OnPropertyChanged("LoggedIn");
            }
        }

        private Visibility _errorVisiblity;

        public Visibility ErrorVisibility
        {
            get { return _errorVisiblity; }
            set {
                _errorVisiblity = value;
                OnPropertyChanged("ErrorVisibility");
            }
        }


        public String? Username { get; set; }

        public String Password { private get; set; }

        public int UserId { get; set; }

        public String? ChatLogName { get; set; }

        private String _error;

        public String Error
        {
            get { return _error; }
            set {
                _error = value;
                OnPropertyChanged("Error");
            }
        }



        public MainViewModel()
        {
            Messages = new ObservableCollection<MessageModel>();

            Client = new ChatGPTClient();

            Directory.CreateDirectory("./ChatLogs");
            

            DbContext = new UserDbContext();
          //  DbContext.Database.EnsureDeleted();
            DbContext.Database.EnsureCreated();

            MessagesEnabled = false;
            LoginVisibility = Visibility.Visible;
            LoggedIn = Visibility.Hidden;
            SignupVisibility = Visibility.Hidden;
            ErrorVisibility = Visibility.Collapsed;
            bool gettingResponse = false;

            SwitchToLoginCommand = new RelayCommand(o =>
            {
                Username = "";
                Password = "";
                Error = "";
                ErrorVisibility = Visibility.Collapsed;
                LoginVisibility = Visibility.Visible;
                SignupVisibility = Visibility.Hidden;
            });

            SwitchToSignupCommand = new RelayCommand(o =>
            {
                Username = "";
                Password = "";
                Error = "";
                ErrorVisibility = Visibility.Collapsed;
                LoginVisibility = Visibility.Hidden;
                SignupVisibility = Visibility.Visible;
            });

            SendCommand = new RelayCommand(async o =>
            {
                if(Message != "" && Message != null && !gettingResponse)
                {
                    var NewMessage = new MessageModel
                    {
                        Username = Username,
                        ImageSource = "./Icons/user.png",
                        Message = Message,
                        Time = DateTime.Now,
                        IsNativeOrigin = true,
                        FirstMessage = true
                    };

                    Messages.Add(NewMessage);
                    Message = "";
                    gettingResponse = true;
                    try
                    {
                        var response = await Client.Ask(Message);
                        var NewMessageBot = new MessageModel
                        {
                            Username = "ChatBot",
                            ImageSource = "./Icons/bot.png",
                            Message = response,
                            Time = DateTime.Now,
                            IsNativeOrigin = true,
                            FirstMessage = true
                        };
                        Messages.Add(NewMessageBot);

                    }catch(ChatGPTClientException ex)
                    {
                        if (ex.Message.Contains("code: TooManyRequests"))
                        {
                            var StartIndex = ex.Message.IndexOf("\"message\": \"") + 12;
                            var EndIndex = ex.Message.IndexOf("\"", StartIndex);
                            var Msg = ex.Message.Substring(StartIndex, EndIndex - StartIndex);
                            var BotMessage = new MessageModel
                            {
                                Username = "ChatBot",
                                ImageSource = "./Icons/bot.png",
                                Message = Msg,
                                Time = DateTime.Now,
                                IsNativeOrigin = false,
                                FirstMessage = true
                            };
                            Messages.Add(BotMessage);
                        }
                        else
                        {
                            new Log
                            {
                                Message = ex.Message
                            }.SaveLog(DbContext, LogTypeEnum.ERROR);
                        }
                    }catch(Exception ex)
                    {
                        new Log
                        {
                            Message = "chyba pri \n " + ex.Message
                        }.SaveLog(DbContext, LogTypeEnum.ERROR);
                    }
                    gettingResponse= false;
                    
                }
            });

            SignupCommand = new RelayCommand(o =>
            {
            if (!String.IsNullOrEmpty(Username) && !String.IsNullOrEmpty(Password))
                {
                    try
                    {
                        UserId = Program.CreateUser(DbContext, Username, Password);
                        if (UserId == -1)
                        {
                            Error = "User with this username already exists";
                            ErrorVisibility = Visibility.Visible;
                            new Log
                            {
                                Message = "User already exists"
                            } .SaveLog(DbContext, LogTypeEnum.INFO);
                        }
                        else
                        {
                            Error = "";
                            ErrorVisibility = Visibility.Hidden;
                            SignupVisibility = Visibility.Hidden;
                            LoggedIn = Visibility.Visible;
                            MessagesEnabled = true;
                            ChatLogName = "./ChatLogs/" + Username + "_" + UserId + "_" + DateTime.Now.Ticks + ".json";
                            ChatLogFile = File.CreateText(ChatLogName);
                            ChatLogFile.AutoFlush = true;
                        }
                    }catch (Exception ex) {
                        new Log
                        {
                            Message = "Error while preparing chat " + ex.Message
                        } .SaveLog(DbContext, LogTypeEnum.ERROR);
                    }
                    
                }
            });

            CloseCommand = new RelayCommand(o =>
            {
                if(ChatLogFile != null && Messages.Count != 0)
                {
                    ChatLogFile.Write(JsonSerializer.Serialize(Messages));
                    try
                    {
                        var response = Program.CreateChat(DbContext, UserId, ChatLogName);
                    }
                    catch (Exception ex) { 
                        new Log
                        {
                            Message = "Create chat error " + ex.Message
                        } .SaveLog(DbContext, LogTypeEnum.ERROR);
                    }
                    
                    
                }
                Application.Current.Shutdown();
            });
        }
    }
}
