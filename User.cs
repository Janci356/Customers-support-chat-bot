using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Customers_support_chat_bot;

public class User
{
    public int UserId { get; set; } // Primary key

    public string Login { get; set; }

    public string Password { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Log> Logs { get; } = new HashSet<Log>();
    public virtual ICollection<Chat> Chats { get; } = new HashSet<Chat>();

}
