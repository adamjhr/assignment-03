namespace Assignment3.Entities;

using Microsoft.EntityFrameworkCore;
public partial class KanbanContext : DbContext
{
    public KanbanContext(DbContextOptions<KanbanContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Task> Tasks { get; set; }
    // Eller: public virtual DbSet<Task> Tasks { get; set; } = null
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        // modelBuilder.Entity<Task>(entity => {
        //     // entity.Property(e => e.Title).HasMaxLength(100)
        // });

        // modelBuilder.Entity<User>(entity => {
            
        // })

        // modelBuilder.Entity<Tag>(entity => {
            
        // })
    }

        
}

    