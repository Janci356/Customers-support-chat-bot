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
        catch (Exception)
        {
            return -1;
        }
    }

    // Find user by UserId
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



    // Method to get all logs for a given UserId
    private static IEnumerable<Log> GetLogsByUserId(DbContext dbContext, int userId)
    {
        return dbContext.Set<Log>().Where(l => l.UserId == userId).ToList();
    }

    // Method to get all logs for a given UserId
    private static IEnumerable<Chat> GetChatsByUserId(DbContext dbContext, int userId)
    {
        return dbContext.Set<Chat>().Where(l => l.UserId == userId).ToList();
    }



    // Method to add a log to the user's Logs collection and save to the database
    // if not success, returns false
    public Boolean AddLogToUserAndDB(DbContext dbContext, Log newLog)
    {
        try
        {
            // Set the user ID for the new log
            newLog.UserId = this.UserId;
            // Save into DB
            newLog.SaveLog(dbContext);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    // Saves Chat into DB and into Users collection Chats
    // if not success, returns false
    public Boolean AddChatToUserAndDB(DbContext dbContext, Chat newChat)
    {
        try
        {
            // Set the user ID for the new chat
            newChat.UserId = this.UserId;

            // Add the chat to the user's Chats collection
            Chats.Add(newChat);

            // Save chat to the database
            dbContext.SaveChanges();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

}
