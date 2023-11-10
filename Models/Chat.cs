using System.ComponentModel;

namespace Customers_support_chat_bot.Models;
public class Chat
{
    public int ChatId { get; set; } // Primary key

    public string? ChatPath { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime LastChangedAt { get; set; }

    // Foreign key property
    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}

