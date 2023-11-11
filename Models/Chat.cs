using System.ComponentModel;
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

    public int SaveChat(DbContext dbContext)
    {
        try
        {
            dbContext.Set<Chat>().Add(this);
            dbContext.SaveChanges();
            return this.ChatId; // Assuming ChatId is set by the database after insertion
        }
        catch (Exception)
        {
            return -1;
        }
    }
}

