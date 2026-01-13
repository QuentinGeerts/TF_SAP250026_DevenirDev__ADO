/*
*  Démonstration ADO
*/

// 1.  Projet Base de données SQL Server

// 1.1.  Création d'un projet SQL Server
// 1.2.  Création des tables (colonnes, contraintes, indexes, triggers, ...)
// 1.3.  Création des vues
// 1.4.  Création des procédures stockées
// 1.5.  Création des scripts de déploiement

// 2.  Espace de noms ADO.NET

// Pour importer Microsoft.Data.SqlClient, installer le package NuGet:
// a. Clic droit sur le projet => Manage NuGet Packages...
// b. Onglet Browse => Rechercher "Microsoft.Data.SqlClient"
// c. Sélectionner le package => Install
using DemoADO.Models;
using Microsoft.Data.SqlClient;
using System.Data;

// 3.  Notion de connexion

// 3.1.  Connection String
// Chaîne de connexion contenant les paramètres nécessaires pour se connecter
// à une base de données (serveur, base, authentification, etc.)

// Pour récupérer la connectionString:
// a. Sur Visual Studio
// b. Server Explorer (CTRL + ALT + S)
// c. Data Connections
//  => Add Connection...
//  => Data Source: Microsoft Sql Server
//  => Entrez le nom du serveur + Trust Server Certificate
//  => Sélectionnez la DB sur laquelle vous souhaitez vous connecter => OK
// d. Clic droit sur la connection => Properties (F4) => Copiez le champ "Connection String"

string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DemoADO;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

// 3.2.  Classe SqlConnection
// Représente une connexion ouverte à une base de données SQL Server.
// Doit être ouverte avant toute opération et fermée après utilisation.
// Hérite de DBConnection et implémente l'interface IDisposable

Console.WriteLine($"\n3.2. Classe SqlConnection\n");

SqlConnection sqlConnection = new SqlConnection(connectionString);

Console.WriteLine($"State: {sqlConnection.State}");
sqlConnection.Open();
Console.WriteLine($"State: {sqlConnection.State}");
// Si une exception survient ici, sqlConnection.Close() n'est jamais appelé
// La connexion reste bloquée dans le pool ! => utilisation d'un using () { ... }
sqlConnection.Close();
Console.WriteLine($"State: {sqlConnection.State}");

// 3.3.  Instruction using
// using() garantit la fermeture automatique de la connexion (appel de Dispose())
// même en cas d'exception, évitant ainsi les fuites de ressources et l'épuisement
// du pool de connexions.

Console.WriteLine($"\n3.3. Instruction Using\n");

using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    // Exception ou pas, Dispose() est appelé automatiquement
    // La connexion retourne dans le pool

    Console.WriteLine($"State: {connection.State}");
    Console.WriteLine($"Data Source: {connection.DataSource}");
    Console.WriteLine($"Database: {connection.Database}");

    // connection.Close();
}

// 4.  Classe SqlCommand
// Représente une requête SQL ou une procédure stockée.
// Permetd'envoyer une requête (SELECT, UPDATE, INSERT, DELETE, ...)

Console.WriteLine($"\n4. Classe SqlCommand\n");

using (SqlConnection connection = new SqlConnection(connectionString))
{
    // a. Constructeur vide
    using (SqlCommand command = new SqlCommand())
    {
        command.CommandText = "SELECT * FROM Student";
        command.Connection = connection;

        //connection.Open();
        // ... 
    }

    // b. En fournissant la commande directement
    using (SqlCommand command = new SqlCommand("SELECT * FROM Student"))
    {
        command.Connection = connection;

        //connection.Open();
        // ...
    }

    // c. En fournissant la commande directement et la connectionString
    using (SqlCommand command = new SqlCommand("SELECT * FROM Student", connection))
    {
        //connection.Open();
        // ...
    }

    // d. En utilisant la méthode CreateCommand de votre connexion
    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "SELECT * FROM Student";
        //connection.Open();
        // ... 
    }
}

// 5.  Mode "Connecté"

// 5.1.  Méthode ExecuteScalar
// Permet d'envoyer une requête et de retourner UNE seule donnée. (Une colonne dans une seule ligne)
// Utile pour récupérer une donnée telle que un id, count, max, min, ...

Console.WriteLine($"\n5.1. Méthode ExecuteScalar\n");

using (SqlConnection connection = new SqlConnection(connectionString))
{
    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "SELECT [Email] FROM [dbo].[User] WHERE [Id] = 1";

        connection.Open();
        string email = (string)command.ExecuteScalar();
        connection.Close();

        Console.WriteLine($"Email: {email}");
    }
}

// 5.2.  Méthode ExecuteReader


// C#: null
// SQL: NULL
// ADO: DBNull.Value

Console.WriteLine($"\n5.2. Méthode ExecuteReader");

using (SqlConnection connection = new SqlConnection(connectionString))
{
    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "SELECT [Id], [Email], [Lastname], [Firstname] FROM [dbo].[V_User]";

        connection.Open();
        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                int id = (int)reader["Id"];
                string email = (string)reader[1];
                string? lastname = reader["Lastname"] as string;
                string? firstname = reader["Firstname"] is DBNull ? null : (string)reader["Firstname"];

                Console.WriteLine($"Id: {id}, email: {email}, lastname: {lastname}, firstname: {firstname}");
            }
        }
        connection.Close();
    }
}

// 5.3.  Méthode ExecuteNonQuery

Console.WriteLine($"\n5.3. Méthode ExecuteNonQuery\n");

using (SqlConnection connection = new SqlConnection(connectionString))
{
    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "UPDATE [dbo].[User] SET [Email] = 'quentin.geerts@cognitic.be' WHERE [Id] = 1";

        connection.Open();
        int nbAffectedRows = command.ExecuteNonQuery();
        connection.Close();

        Console.WriteLine($"Nombre de ligne affectée: {nbAffectedRows}");

        if (nbAffectedRows > 0)
        {
            Console.WriteLine($"Modification effectuée.");
        }
        else
        {
            Console.WriteLine($"Aucune modification effectuée.");
        }
    }
}


// 6.  Mode "déconnecté"

// 6.1.  Classe SqlDataAdapter

Console.WriteLine($"\n6.1. Classe SqlDataAdapter\n");

DataSet dataSet = new DataSet();
DataTable dataTable = new DataTable();

using (SqlConnection connection = new SqlConnection(connectionString))
{
    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "SELECT * FROM [V_User]";

        SqlDataAdapter adapter = new SqlDataAdapter(command);
        adapter.Fill(dataSet); // Ouverture automatique de la connexion
        adapter.Fill(dataTable);
    } // Fermeture automatique de la connexion
}

// Parcourt du DataSet
if (dataSet.Tables.Count > 0)
{
    foreach (DataRow row in dataSet.Tables[0].Rows)
    {
        int id = (int)row["Id"];
        string email = (string)row["Email"];

        Console.WriteLine($"{id} {email}");
    }
}

// Parcourt du DataTable
if (dataTable.Rows.Count > 0)
{
    foreach(DataRow row in dataTable.Rows)
    {
        int id = (int)row["Id"];
        string email = (string)row["Email"];
        string? lastname = row["Lastname"] is DBNull ? null : (string)row["Lastname"];

        Console.WriteLine($"{id} {email} {lastname}");
    }
}



// 7.  Mot-clef "output"

Console.WriteLine($"\n7. Mot-clef \"output\"\n");

using (SqlConnection connection = new SqlConnection(connectionString))
{
    using (SqlCommand cmd = connection.CreateCommand())
    {
        cmd.CommandText = @"
        INSERT INTO Todo (Title, Description, Status, UserId)
        OUTPUT inserted.Id
        VALUES ('Faire à manger', 'Un plât protéiné', 'To do', 1)";

        connection.Open();
        int id = (int)cmd.ExecuteScalar();
        connection.Close();

        Console.WriteLine($"Nouveau Todo: {id}");
    }
}

using (SqlConnection connection = new SqlConnection(connectionString))
{
    using (SqlCommand cmd = connection.CreateCommand())
    {
        cmd.CommandText = @"
        INSERT INTO Todo (Title, Description, Status, UserId)
        OUTPUT inserted.Id, inserted.Title, inserted.Description, inserted.UserId
        VALUES ('Faire à manger', 'Un plât protéiné', 'To do', 1)";

        connection.Open();
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            while(reader.Read())
            {
                int id = Convert.ToInt32(reader["Id"]);
                string title = (string)reader["Title"];
                string? description = reader["Description"] is DBNull ? null : (string)reader["Description"];
                int userId = (int)reader["UserId"];

                Console.WriteLine($"{id} {title} {description} {userId}");
            }
        }
        connection.Close();
    }
}