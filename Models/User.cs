using Customers_support_chat_bot.enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Customers_support_chat_bot.Models;

public class User
{
    public int UserId { get; set; } // Primary key

    public string Login { get; set; }

    public string Password { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<Log> Logs { get; } = new HashSet<Log>();
    public virtual ICollection<Chat> Chats { get; } = new HashSet<Chat>();

    //----------------------------------------------------------------------------------------
    // DELETE LOG AND CHAT FROM USER AND DB

    // Method to delete a log from the database and remove it from the user's collection
    // return true or false
    public bool DeleteLogAndSave(DbContext dbContext, int logToDeleteId)
    {
        try
        {
            // check if log is in db
            var logToDelete = Log.FindById(dbContext, logToDeleteId);

            if (logToDelete != null)
            {
                // Remove the log from the user's collection
                Logs.Remove(logToDelete);

                // Remove the log from the database
                dbContext.Set<Log>().Remove(logToDelete);
                dbContext.SaveChanges();
                return true;
            }

            return false;
        }
        catch (Exception e)
        {
            new Log
            {
                Message = "Error delete log and chat from user and db " + e.Message
            } .SaveLog(dbContext, LogTypeEnum.ERROR);
            return false;
        }
    }

    // Method to delete a chat from the database and remove it from the user's collection
    // return true or false
    public bool DeleteChatAndSave(DbContext dbContext, int chatToDeleteId)
    {
        try
        {
            // check if chat is in db
            var chatToDelete = Chat.FindById(dbContext, chatToDeleteId);

            if (chatToDelete != null)
            {
                // Remove the chat from the user's collection
                Chats.Remove(chatToDelete);

                // Remove the log from the database
                dbContext.Set<Chat>().Remove(chatToDelete);
                dbContext.SaveChanges();
                return true;
            }

            return false;
        }
        catch (Exception e)
        {
            new Log
            {
                Message = "Error delete a chat from the database and remove it from the user's collection " + e.Message
            } .SaveLog(dbContext, LogTypeEnum.ERROR);
            return false;
        }
    }

    //----------------------------------------------------------------------------------------
    // GET USER BY ID OR LOGIN

    // Find user by UserId return User or null
    public static User? FindById(DbContext dbContext, int userId)
    {
        return dbContext.Set<User>().Find(userId);
    }

    // Find user by Login
    // Returns null if no such User in db
    public static User? FindByLogin(DbContext dbContext, string login)
    {
        return dbContext.Set<User>().FirstOrDefault(u => u.Login == login);
    }


    //----------------------------------------------------------------------------------------
    // SAVE USER INTO DB

    // Save the user to the database with associated logs and return the UserId or -1 if unsuccessful
    public int SaveUser(DbContext dbContext)
    {
        try
        {
            // Add the user to the context and save changes
            dbContext.Set<User>().Add(this);
            dbContext.SaveChanges();
            return this.UserId; // Assuming UserId is set by the database after insertion
        }
        catch (Exception e)
        {
            new Log
            {
                Message = "Error Save the user to the database " + e.Message
            } .SaveLog(dbContext, LogTypeEnum.ERROR);
            return -1;
        }
    }


    //----------------------------------------------------------------------------------------
    // GET LOGS AND CHATS FOR USER

    // Method to get all logs for a given UserId
    public IEnumerable<Log> GetLogs(DbContext dbContext)
    {
        return dbContext.Set<Log>().Where(l => l.UserId == UserId).ToList();
    }

    // Method to get all logs for a given UserId
    public IEnumerable<Chat> GetChats(DbContext dbContext)
    {
        return dbContext.Set<Chat>().Where(l => l.UserId == UserId).ToList();
    }


    //----------------------------------------------------------------------------------------
    // ADD LOG AND CHAT TO USER AND DB

    // Method to add a log to the user's Logs collection and save to the database
    // returns LogId or -1
    public int AddLogToUserAndDB(DbContext dbContext, Log newLog, LogTypeEnum logType)
    {
        // Set the user ID for the new log
        newLog.UserId = this.UserId;

        // Add the chat to the user's Chats collection
        Logs.Add(newLog);
        // Save into DB and return false or true
        return newLog.SaveLog(dbContext, logType);
    }

    // Saves Chat into DB and into Users collection Chats
    // if not success returns -1, else ChatId
    public int AddChatToUserAndDB(DbContext dbContext, Chat newChat)
    {
        // Set the user ID for the new chat
        newChat.UserId = this.UserId;
        newChat.User = this;

        // Add the chat to the user's Chats collection
        Chats.Add(newChat);

        // Save into DB and return false or true
        return newChat.SaveChat(dbContext);
    }
}