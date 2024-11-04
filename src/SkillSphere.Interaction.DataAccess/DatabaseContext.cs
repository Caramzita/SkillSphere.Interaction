using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SkillSphere.Interaction.Core;
using System.Reflection;

namespace SkillSphere.Interaction.DataAccess;

public class DatabaseContext : DbContext
{
    /// <summary>
    /// Логгер.
    /// </summary>
    private readonly ILogger<DatabaseContext> _logger;

    public DbSet<Reaction> Reactions { get; set; }

    public DbSet<Comment> Comments { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options, ILogger<DatabaseContext> logger) 
        : base(options)
    {
        _logger = logger;
    }

    /// <summary>
    /// Настраивает сущности модели с использованием конфигураций из сборки.
    /// </summary>
    /// <param name="modelBuilder">Построитель модели, используемый для настройки сущностей.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    /// <summary>
    /// Настраивает параметры подключения к базе данных.
    /// </summary>
    /// <param name="optionsBuilder">Построитель параметров, используемый для настройки подключения.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            _logger.LogWarning("DbContextOptionsBuilder is not configured.");
        }

        optionsBuilder.EnableSensitiveDataLogging()
                      .LogTo(log => _logger.LogInformation(log));
    }
}
