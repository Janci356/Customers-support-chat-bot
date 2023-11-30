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
                // "heslo"
            new User { UserId = 1, Login = "Meat",
                Password = "AAFsdQfjQSD2PImTtSrPQ82xm0YYkIQA4OwkHtYTR4WifSaCKT56Rp81UyOA74CYj9uFq5Ss5VUMoJ0SwBIGlg==",
                Salt = "NE7s45Ismfn0njMsHg/Up8euy01182UwnQc2ReVfFSJvseBwqri2jwmPOpqlC1pCDIkfj/rIBo3NT3lJv08mhdqt/qMxyDK8bx1qK4NxSdtyHdBq73/dtAf4tEyhN6WtLiSKM9N3nftGVjglAUKzJzAvqpQJixchnicNSMDRSbk=",
                CreatedAt = DateTime.UtcNow },
            // "heslo1"
            new User { UserId = 2, Login = "Fish",
                Password = "X1dAsDLKz84UrAw8Wzxs52G6c3DEosBLiDnecNq7j3bUpMf4QIs8FdfViNIHXd41OUMqI0Nl8AuwBxrDpy9gAg==",
                Salt = "lEbhdQ3W01Th3gTuT3oRvAx9RLb7dIkjEiW3mafzcwwqfrkOHtks0J/SkR6GnjRDB3fPLr59V2SH4AbKumV6Vv44SFxkAQBeYkyJAxg7gbLxqD/kDjLfO7C/d16Q2tMnLFPZppf40Mrvvkf2a33pCKxaYjuOLm6kSipsLTpL+iY=",
                CreatedAt = DateTime.UtcNow },
            // "heslo"
            new User { UserId = 3, Login = "Cheese",
                Password = "rK8S/e7/O+mqyq5Y1hsgQF3BZRdIePwTXfhi8e3v4M7Ny5A0rFVL6is2GO0vDh6MqhTsV/SKoJRA+rsTClVmyg==",
                Salt = "sJI+JrLmi1373AWUiD5NJN1n8Z98axHMG87fVnWq/cc/HltzJ9F+x7DYJr/fp6AZ/zLh+zaZFPY/rqeei5PvqVHh0z97v9Jec2YZWfAnQYAACfwW/pqF2KR3+uEMAxWDb0NnZJiehEHdLV2MVdN6pHmJ6g6Xc9WpfLcWPb75Yjw=",
                CreatedAt = DateTime.UtcNow }
            };

            modelBuilder.Entity<User>().HasData(users);
        }
        catch (Exception ex)
        {
            throw new DBException(ex.Message);
        }
    }
}
