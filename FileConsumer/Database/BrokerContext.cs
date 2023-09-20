using Microsoft.EntityFrameworkCore;

namespace FileConsumer.Database;

public class BrokerContext : DbContext
{
    
    private readonly string _connectionString;

    public BrokerContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public BrokerContext(DbContextOptions<BrokerContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    public DbSet<Message> Messages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }
    }
}