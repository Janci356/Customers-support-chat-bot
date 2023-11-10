using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Customers_support_chat_bot;

public class Log
{
    public int LogId { get; set; } // Primary key

    public string? LogPath { get; set; }

    public DateTime CreatedAt { get; set; }

    // Foreign key property
    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
