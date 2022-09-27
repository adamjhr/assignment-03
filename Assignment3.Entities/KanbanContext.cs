using Assignment3.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

namespace Assignment3.Entities;

public class KanbanContext : DbContext
{
    public KanbanContext(DbContextOptions<KanbanContext> options) : base(options) {
    }
    
    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<Task>().HasMany<Tag>(t => t.Tags).WithMany(t => t.Tasks);
        
        modelBuilder.Entity<User>().HasIndex(u => u.Email)
            .IsUnique();
        modelBuilder.Entity<Tag>().HasIndex(t => t.Name)
            .IsUnique();
        modelBuilder.Entity<Tag>().HasMany<Task>(t => t.Tasks).WithMany(t => t.Tags);

        modelBuilder.Entity<Task>()
            .Property(task => task.State)
            .HasConversion(state => state.ToString(), state => Enum.Parse<State>(state));
        

    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     var configuration = new ConfigurationBuilder()
    //         .AddUserSecrets(typeof(KanbanContext).Assembly, true)
    //         .Build();
    //     var connectionString = configuration.GetConnectionString("ConnectionString");
        
    //     optionsBuilder.UseLazyLoadingProxies().UseNpgsql(connectionString);
        
    // }
}
