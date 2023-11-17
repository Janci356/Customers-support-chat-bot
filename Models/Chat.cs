using System.ComponentModel;
using Customers_support_chat_bot.enums;
using Microsoft.EntityFrameworkCore;

namespace Customers_support_chat_bot.Models;

public class Chat
{
    public int ChatId { get; set; } // Primary key

    public string? ChatPath { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? LastChangedAt { get; set; } = DateTime.UtcNow;

    // Foreign key property
    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;


    //----------------------------------------------------------------------------------------
    // UPDATE LAST CHANGED TIME

    public void UpdateLastChangedAt()
    {
        LastChangedAt = DateTime.UtcNow;
    }

    //----------------------------------------------------------------------------------------
    // GET CHAT BY ID

    // Find chat by ChatId, return Chat or null
    public static Chat? FindById(DbContext dbContext, int chatId)
    {
        return dbContext.Set<Chat>().Find(chatId);
    }


    //----------------------------------------------------------------------------------------
    // SAVE CHAT TO DB

    public int SaveChat(DbContext dbContext)
    {
        try
        {
            dbContext.Set<Chat>().Add(this);
            dbContext.SaveChanges();
            return this.ChatId; // Assuming ChatId is set by the database after insertion
        }
        catch (Exception e)
        {
            new Log
            {
                Message = "Error saving chat to database: " + e.Message
            } .SaveLog(dbContext, LogTypeEnum.ERROR);
            return -1;
        }
    }
}