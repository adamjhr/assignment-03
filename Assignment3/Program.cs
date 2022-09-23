// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;


var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
// var connectionString = configuration.GetConnectionString("Kanban"); // Eller skal det være "ConnectionString"
var connectionString = "User ID=postgres;Password=adam123;Data Source=localhost,5432;Database=imdb;Pooling=true;Min Pool Size=0; Max Pool Size=100;TrustServerCertificate=true;Timeout=1000000";



using var connection = new SqlConnection(connectionString);
using var getTable = new SqlCommand(@"SELECT * FROM testtable", connection);

connection.Open();

using var reader = getTable.ExecuteReader();

while (reader.Read())
{
    Console.WriteLine(reader.GetString(0));
}

connection.Close();


// var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>();
// optionsBuilder.UseSqlServer(connectionString);
