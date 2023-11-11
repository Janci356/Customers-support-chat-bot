
using Catalyst;
using Customers_support_chat_bot;
using Customers_support_chat_bot.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

class Program
{
    // Function to create a new user with the given parameters and save to the database
    private static int CreateUser(DbContext dbContext, string login, string password)
    {
        // Create a new user
        User newUser = new User
        {
            Login = login,
            Password = password,
        };

        // Save the user to the database and return UserId or -1 if unsuccessful
        return newUser.SaveUser(dbContext);
    }

    // Save the log to the database and user, If User isn't in db or can't save log into db return false
    private static bool CreateLog(DbContext dbContext,int userId, string logPath)
    {
        // Create a new log
        Log newLog = new Log
        {
            LogPath = logPath
        };

        return User.FindById(dbContext, userId)?.AddLogToUserAndDB(dbContext, newLog) != null;
    }


    // Save the chat to the database and user, If User isn't in db or can't save chat into db return false
    private static bool CreateChat(DbContext dbContext, int userId, string chatPath)
    {
        Chat newChat = new Chat
        {
            ChatPath = chatPath
        };

        return User.FindById(dbContext, userId)?.AddChatToUserAndDB(dbContext, newChat) != null;
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




    static async Task Main(string[] args)
    {
        NLPModel model = new NLPModel();
        Console.WriteLine("Type something:\n");
        var userInput = Console.ReadLine();
        IDocument processedInput = await model.Process(userInput is null ? "" : userInput);
        Console.Write(processedInput.ToJson());
    }
}