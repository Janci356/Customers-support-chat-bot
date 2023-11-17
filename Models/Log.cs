using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Customers_support_chat_bot.enums;
using Microsoft.EntityFrameworkCore;

namespace Customers_support_chat_bot.Models;

public class Log
{
    public int LogId { get; set; } // Primary key

    public string? Message { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign key property
    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;


    //----------------------------------------------------------------------------------------
    // GET LOG BY ID

    // Find log by LogId, return Log or null
    public static Log? FindById(DbContext dbContext, int logId)
    {
        return dbContext.Set<Log>().Find(logId);
    }

    //----------------------------------------------------------------------------------------
    // SAVE LOG TO DB

    public int SaveLog(DbContext dbContext, LogTypeEnum logType)
    {
        try
        {
            dbContext.Set<Log>().Add(this);
            dbContext.SaveChanges();
            switch (logType)
            {
                case LogTypeEnum.INFO:
                    Logger.GetInstance().LogInformation(Message);
                    break;
                case LogTypeEnum.ERROR:
                    Logger.GetInstance().LogError(Message);
                    break;
                default:
                    Logger.GetInstance().LogInformation(Message);
                    break;
            }
            return LogId; // Assuming LogId is set by the database after insertion
        }
        catch (Exception e)
        {
            Logger.GetInstance().LogInformation(e.Message);
            return -1;
        }
    }
}