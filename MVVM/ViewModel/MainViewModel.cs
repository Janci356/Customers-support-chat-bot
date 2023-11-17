﻿using Customers_support_chat_bot.Core;
using Customers_support_chat_bot.MVVM.Model;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;

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

        private Visibility _loggedIn;

        public Visibility LoggedIn
        {
            get { return _loggedIn; }
            set {
                _loggedIn = value;
                OnPropertyChanged("LoggedIn");
            }
        }

        public String Username { get; set; }

        public String Password { private get; set; }

        public int UserId { get; set; }

        public String ChatLogName { get; set; }


        public MainViewModel()
        {
            Messages = new ObservableCollection<MessageModel>();

            Client = new ChatGPTClient();

            Directory.CreateDirectory("./ChatLogs");
            

            DbContext = new UserDbContext();
            DbContext.Database.EnsureDeleted();
            DbContext.Database.EnsureCreated();

            MessagesEnabled = false;
            LoginVisibility = Visibility.Visible;
            LoggedIn = Visibility.Hidden;

            SendCommand = new RelayCommand(async o =>
            {
                if(Message != "" && Message != null)
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

                    var response = await Client.Ask(Message);

                    if(response.Contains("code: TooManyRequests"))
                    {
                        var StartIndex = response.IndexOf("\"message\": \"") + 12;
                        var EndIndex = response.IndexOf("\"", StartIndex);
                        var Msg = response.Substring(StartIndex, EndIndex - StartIndex);
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

                    Message = "";
                }
            });

            LoginCommand = new RelayCommand(o =>
            {
            if (!String.IsNullOrEmpty(Username) && !String.IsNullOrEmpty(Password)) 
                { 
                    UserId = Program.CreateUser(DbContext, Username, Password); 
                    if(UserId == -1)
                    {
                        // TODO
                    }
                    else
                    {
                        LoginVisibility = Visibility.Hidden;
                        LoggedIn = Visibility.Visible;
                        MessagesEnabled = true;
                        ChatLogName = "./ChatLogs/" + Username + "_" + UserId + "_" + DateTime.Now.Ticks + ".json";
                        ChatLogFile = File.CreateText(ChatLogName);
                        ChatLogFile.AutoFlush = true;
                    }
                }
            });

            CloseCommand = new RelayCommand(o =>
            {
                if(ChatLogFile != null && Messages.Count != 0)
                {
                    ChatLogFile.Write(JsonSerializer.Serialize(Messages));
                    var response = Program.CreateChat(DbContext, UserId, ChatLogName);
                    Console.WriteLine();
                }
                Application.Current.Shutdown();
            });
        }
    }
}