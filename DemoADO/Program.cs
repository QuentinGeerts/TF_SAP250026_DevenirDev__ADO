/*
 *  Démonstration ADO
 */


// 1.  Création d'un projet: Projet de base de données SQL Server
// 1.1.  Création des tables
// 1.2.  Création des vues
// 1.3.  Création des procédures stockées
// 1.4.  Création des scripts de post-déploiement

using Microsoft.Data.SqlClient;

// 2.  Notion de connexion

// 2.1.  Connection String


string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DemoADO;Integrated Security=True;Trust Server Certificate=True";

// 2.2.  Classe SqlConnection
// => Importer le package nuget Microsoft.Data.SqlClient

SqlConnection sqlConnection = new SqlConnection(connectionString);

Console.WriteLine($"State: {sqlConnection.State}");
sqlConnection.Open();
Console.WriteLine($"State: {sqlConnection.State}");
sqlConnection.Close();
Console.WriteLine($"State: {sqlConnection.State}");


// 3.  L'instruction using () { ... }

Console.WriteLine("\n3. L'instruction using () { ... }\n");

using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    // ... 

    Console.WriteLine($"State: {connection.State}");
    Console.WriteLine($"Database: {connection.Database}");
    Console.WriteLine($"DataSource: {connection.DataSource}");

} // connection.Close();
