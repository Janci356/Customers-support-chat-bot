
using Catalyst;
using Customers_support_chat_bot;
using Customers_support_chat_bot.Models;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System;
using System.Linq;
using Microsoft.ML;

class Program
{
    // Function to create a new user with the given parameters and save to the database
    private static int CreateUser(DbContext dbContext, string login, string password)
    {
        // if login is already taken
        if (GetUser(dbContext, login) != null) return -1;

        // Create a new user
        User newUser = new User
        {
            Login = login,
            Password = password,
        };

        // Save the user to the database and return UserId or -1 if unsuccessful
        return newUser.SaveUser(dbContext);
    }

    // Save the log to the database and user, If User isn't in db or can't save log into db return -1 
    // return LogId if success
    private static int CreateLog(DbContext dbContext,int userId, string logPath)
    {
        var user = User.FindById(dbContext, userId);

        if (user == null)
        {
            //Console.WriteLine($"User with ID {userId} not found.");
            return -1; // or throw an exception, depending on your error handling strategy
        }

        // Create a new log
        Log newLog = new Log
        {
            LogPath = logPath
        };
        return user.AddLogToUserAndDB(dbContext, newLog);        
    }


    // Save the chat to the database and user, If User isn't in db  or can't save chat into db return -1 
    // return ChatId if success
    private static int CreateChat(DbContext dbContext, int userId, string chatPath)
    {
        var user = User.FindById(dbContext, userId);

        if (user == null)
        {
            //Console.WriteLine($"User with ID {userId} not found.");
            return -1; // or throw an exception, depending on your error handling strategy
        }
        Chat newChat = new Chat
        {
            ChatPath = chatPath
        };

        return user.AddChatToUserAndDB(dbContext, newChat);
    }

    // Get All logs from db for given userid, If User isn't in db return null,
    // If user hasn't got logs, return empty (i think)
    private static IEnumerable<Log>? GetUserLogs(DbContext dbContext, int userId)
    {
        return User.FindById(dbContext, userId)?.GetLogs();
    }

    // Get All chats from db for given userid, If User isn't in db return null,
    // If user hasn't got chats return empty (i think)
    private static IEnumerable<Chat>? GetUserChats(DbContext dbContext, int userId)
    {
        return User.FindById(dbContext, userId)?.GetChats();
    }

    // Get user by id
    private static User? GetUser(DbContext dbContext, int userId)
    {
        return User.FindById(dbContext, userId);
    }
    // Get user by login
    private static User? GetUser(DbContext dbContext, string userLogin)
    {
        return User.FindByLogin(dbContext, userLogin);
    }
    private static Log? GetLog(DbContext dbContext, int logId)
    {
        return Log.FindById(dbContext, logId);
    }
    private static Chat? GetChat(DbContext dbContext, int chatId)
    {
        return Chat.FindById(dbContext, chatId);
    }

    // Update parameter LastChangedAt to UtcNow, returns true or false if chat no exist
    private static bool UpdateChat(DbContext dbContext, int chatId)
    {
        var chat = Chat.FindById(dbContext, chatId);

        if (chat != null)
        {
            chat.UpdateLastChangedAt();
            return true; // Update successful
        }
        else
        {
            return false; // Chat not found
        }
    }

    // Remove Log from db and user's collection, returns true or false
    private static bool RemoveLog(DbContext dbContext, int logId)
    {
        var log = Log.FindById(dbContext, logId);

        if (log != null && log.User != null)
        {
            // Remove log from user and db
            return log.User.DeleteChatAndSave(dbContext, logId);
        }
        else
        {
            return false; // User or Log not found or couldn't be removed
        }
    }

    // Remove Chat from db and user's collection, returns true or false
    private static bool RemoveChat(DbContext dbContext, int chatId)
    {
        var chat = Chat.FindById(dbContext, chatId);

        if (chat != null && chat.User != null)
        {
            // Remove chat from user and db
            return chat.User.DeleteChatAndSave(dbContext, chatId);
        }
        else
        {
            return false; // User or Chat not found or couldn't be removed
        }
    }



    static async Task Main(string[] args)
    {
        NLPModel model = new NLPModel();
        Console.WriteLine("Type something:\n");
        var userInput = Console.ReadLine();
        IDocument processedInput = await model.Process(userInput is null ? "" : userInput);
        Console.Write(processedInput.ToJson());

        using (var context = new UserDbContext())
        {
            // to reset db entries from Context file uncomment line below:
            // context.Database.EnsureDeleted();

            context.Database.EnsureCreated();
        }
    }
}