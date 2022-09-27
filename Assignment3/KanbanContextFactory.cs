using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Assignment3.Entities;
using Microsoft.EntityFrameworkCore.Sqlite;

namespace Assignment3;

public class KanbanContextFactory : IDesignTimeDbContextFactory<KanbanContext> {

    public KanbanContext CreateDbContext(string[] args) {

        var configuration = new ConfigurationBuilder()
            .AddUserSecrets(typeof(KanbanContext).Assembly, true)
            .Build();
        var connectionString = configuration.GetConnectionString("ConnectionString");

        var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>();
        optionsBuilder.UseLazyLoadingProxies().UseNpgsql(connectionString);


        return new KanbanContext(optionsBuilder.Options);
    }

    public KanbanContext CreateDbTestContext(string[] args) {

        var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>();
        optionsBuilder.UseSqlite();

        return new KanbanContext(optionsBuilder.Options);
    }

}