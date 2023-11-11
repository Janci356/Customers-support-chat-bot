using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Customers_support_chat_bot.Models;

public class Log
{
    public int LogId { get; set; } // Primary key

    public string? LogPath { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign key property
    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;

    public int SaveLog(DbContext dbContext)
    {
        try
        {
            dbContext.Set<Log>().Add(this);
            dbContext.SaveChanges();
            return this.LogId; // Assuming LogId is set by the database after insertion
        }
        catch (Exception)
        {
            return -1;
        }
    }
}
