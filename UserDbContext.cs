using Microsoft.EntityFrameworkCore;

namespace Customers_support_chat_bot;

public class UserDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Log> Logs { get; set; }

    public DbSet<Chat> Chats { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Data Source=users.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User { UserId = 0, Login = "Cheese", Password = "111", CreatedAt = DateTime.UtcNow},
            new User { UserId = 1, Login = "Meat", Password = "111", CreatedAt = DateTime.UtcNow },
            new User { UserId = 2, Login = "Fish", Password = "111", CreatedAt = DateTime.UtcNow }
            );

        modelBuilder.Entity<Log>().HasData(
            new Log { LogId = 0, UserId = 1, CreatedAt = DateTime.UtcNow },
            new Log { LogId = 1, UserId = 1, CreatedAt = DateTime.UtcNow },
            new Log { LogId = 2, UserId = 2, CreatedAt = DateTime.UtcNow },
            new Log { LogId = 3, UserId = 1, CreatedAt = DateTime.UtcNow },
            new Log { LogId = 4, UserId = 1, CreatedAt = DateTime.UtcNow },
            new Log { LogId = 5, UserId = 0, CreatedAt = DateTime.UtcNow },
            new Log { LogId = 6, UserId = 0, CreatedAt = DateTime.UtcNow }
           );

        modelBuilder.Entity<Chat>().HasData(
            new Chat { ChatId = 0, UserId = 0, CreatedAt = DateTime.UtcNow, LastChangedAt = DateTime.UtcNow },
            new Chat { ChatId = 1, UserId = 1, CreatedAt = DateTime.UtcNow, LastChangedAt = DateTime.UtcNow },
            new Chat { ChatId = 2, UserId = 2, CreatedAt = DateTime.UtcNow, LastChangedAt = DateTime.UtcNow },
            new Chat { ChatId = 3, UserId = 0, CreatedAt = DateTime.UtcNow, LastChangedAt = DateTime.UtcNow },
            new Chat { ChatId = 4, UserId = 2, CreatedAt = DateTime.UtcNow, LastChangedAt = DateTime.UtcNow }
           );
    }
}
