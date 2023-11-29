using Customers_support_chat_bot.Exceptions;
using Customers_support_chat_bot.Models;
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
        // one to many relationship:
        modelBuilder.Entity<Log>()
        .HasOne(l => l.User)
        .WithMany(u => u.Logs)
        .HasForeignKey(l => l.UserId)
        .IsRequired(false);
        modelBuilder.Entity<Chat>()
            .HasOne(c => c.User)
            .WithMany(u => u.Chats)
            .HasForeignKey(c => c.UserId);

        // primary keys:
        modelBuilder.Entity<User>()
            .HasKey(u => u.UserId);

        modelBuilder.Entity<Log>()
            .HasKey(u => u.LogId);

        modelBuilder.Entity<Chat>()
            .HasKey(u => u.ChatId);

        // is unique:
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Login)
            .IsUnique();

        // starting values:
        try
        {
            var users = new List<User>
            {
                new User { UserId = 1, Login = "Meat", Password = "111", CreatedAt = DateTime.UtcNow },
                new User { UserId = 2, Login = "Fish", Password = "111", CreatedAt = DateTime.UtcNow },
                new User { UserId = 3, Login = "Cheese", Password = "111", CreatedAt = DateTime.UtcNow }
            };

            modelBuilder.Entity<User>().HasData(users);
        }
        catch (Exception ex)
        {
            throw new DBException(ex.Message);
        }
    }
}
