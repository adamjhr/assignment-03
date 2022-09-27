// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;


var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
// var connectionString = configuration.GetConnectionString("Kanban"); // Eller skal det være "ConnectionString"
// var connectionString = "User ID=postgres;Password=adam123;Data Source=localhost,5432;Database=imdb;Pooling=true;Min Pool Size=0; Max Pool Size=100;TrustServerCertificate=true;Timeout=1000000";

//using var connection = new SqlConnection(connectionString);
using var connection = new SqliteConnection("Data source = test.db");

var createTable = connection.CreateCommand();
var getTable = connection.CreateCommand();
var addToTable = connection.CreateCommand();
createTable.CommandText = @"CREATE TABLE test (name varchar(10))";
addToTable.CommandText = @"INSERT INTO test VALUES ('word'), ('word2')";
getTable.CommandText = @"SELECT * FROM test";

connection.Open();

createTable.ExecuteNonQuery();
addToTable.ExecuteNonQuery();

using var reader = getTable.ExecuteReader();

while (reader.Read())
{
    Console.WriteLine(reader.GetString(0));
}

connection.Close();


// var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>();
// optionsBuilder.UseSqlServer(connectionString);
