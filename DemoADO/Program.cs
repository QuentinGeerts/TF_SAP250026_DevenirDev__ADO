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
// Maintient la connexion ouverte pendant la lecture/manipulation des données.
// Utilise SqlDataReader - rapide mais bloque la connexion.
// Idéal pour : lecture séquentielle, grandes quantités de données, opérations rapides.

// 5.1.  Méthode ExecuteScalar
// Exécute la requête et retourne la première colonne de la première ligne du résultat.
// Utilisée pour récupérer une valeur unique (COUNT, MAX, ID, etc.)

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
// Exécute la requête et retourne un SqlDataReader pour lire les lignes de résultat une par
// une de manière séquentielle et en lecture seule

// Classe SqlDataReader
// Fournit un flux de données en lecture seule et permet parcourir les résultats
// d'une requête ligne par ligne de manière performante.
// Doit être fermé après utilisation.

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
// Exécute une commande SQL qui ne retourne pas de résultat (INSERT, UPDATE, DELETE, CREATE TABLE, etc.).
// Retourne le nombre de lignes affectées.

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
// Récupère les données en mémoire (DataSet/DataTable), puis ferme la connexion.
// Utilise SqlDataAdapter - permet manipulation offline, reconnexion pour sauvegarder.
// Idéal pour : binding UI, modifications multiples, travail offline, mise en cache.


// 6.1. Classe SqlDataAdapter
// Sert de pont entre une base de données SQL Server et un DataSet/DataTable en mode déconnecté.
// Remplit les données (Fill) et les met à jour (Update) automatiquement.

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
    foreach (DataRow row in dataTable.Rows)
    {
        int id = (int)row["Id"];
        string email = (string)row["Email"];
        string? lastname = row["Lastname"] is DBNull ? null : (string)row["Lastname"];

        Console.WriteLine($"{id} {email} {lastname}");
    }
}



// 7.  Mot-clef "output"
// Clause SQL Server utilisée dans INSERT, UPDATE, DELETE pour retourner les valeurs insérées/modifiées/supprimées. 
// Permet de récupérer l'ID généré ou les anciennes/nouvelles valeurs sans requête supplémentaire.

// inserted: Valeur APRÈS modification
// deleted: Valeur AVANT modification

// <!> différence entre le mot-clef "output" et le paramètre de sortie d'une procédure stockée <!>
// OUTPUT (procédure stockée) = paramètre de sortie
// OUTPUT(DML) = clause retournant directement les résultats

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
            while (reader.Read())
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


// 8.  Requêtes paramétrées
// Requête SQL utilisant des paramètres (@Param) au lieu de concaténation de chaînes.
// Protège contre les injections SQL, améliore les performances et gère automatiquement les types de données.

/*
 * // MAUVAIS - Concaténation (injection SQL possible)
 * string query = "SELECT * FROM Users WHERE Username = '" + username + "'";
 * 
 * // BON - Requête paramétrée (sécurisée)
 * string query = "SELECT * FROM Users WHERE Username = @Username";
 * cmd.Parameters.AddWithValue("@Username", username);
 */

Console.WriteLine($"\n8. Requêtes paramétrées\n");

int userIdToSearch = 1;

using (SqlConnection c = new SqlConnection(connectionString))
{
    using (SqlCommand cmd = c.CreateCommand())
    {
        cmd.CommandText = "SELECT * FROM V_User WHERE Id = @Id";
        cmd.Parameters.AddWithValue("Id", userIdToSearch);

        c.Open();

        using (SqlDataReader r = cmd.ExecuteReader())
        {
            while (r.Read())
            {
                int id = (int)r["Id"];
                string email = (string)r["Email"];
                string? lastname = r["Lastname"] as string;
                string? firstname = r["Firstname"] as string;

                Console.WriteLine($"User 1: {id} {email} {lastname} {firstname}");
            }
        }

        c.Close();
    }
}


// 9.  Appel de procédure
// Exécution d'une procédure stockée SQL Server via SqlCommand en définissant CommandType = CommandType.StoredProcedure. 
// Permet de passer des paramètres d'entrée/sortie et de récupérer les résultats.
// Nécessite un SqlParameter avec Direction = ParameterDirection.Output pour récupérer la valeur.

Console.WriteLine($"\n9. Appel de procédure\n");

using (SqlConnection c = new SqlConnection(connectionString))
{
    using (SqlCommand cmd = c.CreateCommand ())
    {
        cmd.CommandText = "SP_User_AddUser";
        cmd.CommandType = CommandType.StoredProcedure;

        string email = "geerts.quentin@gmail.com";
        string password = "Test1234=";
        string? lastname = "Geerts";
        string? firstname = null;

        cmd.Parameters.AddWithValue("Email", email);
        cmd.Parameters.AddWithValue("Password", password);
        cmd.Parameters.AddWithValue("Lastname", lastname);
        cmd.Parameters.AddWithValue("Firstname", firstname);

        cmd.Parameters.Add(new SqlParameter("UserId", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        });

        try
        {
            c.Open();
            cmd.ExecuteNonQuery();
            int id = (int)cmd.Parameters["UserId"].Value;
            Console.WriteLine($"Utilisateur créé: {id}");
            c.Close();
        }
        catch (SqlException e)
        {
            Console.WriteLine($"Sql Exception: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erreur inattendue: {e.Message}");
        }

    }
}


/* Ceci n'a pas été fait en cours */

// 10.  Gestion des transactions (Classe SqlTransaction)
// Représente une transaction SQL Server permettant de regrouper plusieurs opérations en une unité atomique.
// Toutes les opérations réussissent (Commit) ou échouent ensemble (Rollback).

Console.WriteLine($"\n9. Gestion des transactions\n");

using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    using (SqlTransaction transaction = connection.BeginTransaction()) // Créé un point de restauration
    {
        int modifiedRows = 0;

        using (SqlCommand command = connection.CreateCommand())
        {
            command.CommandText = "DELETE FROM [dbo].[User] WHERE Id = 2";
            command.Transaction = transaction;

            modifiedRows = command.ExecuteNonQuery();
            Console.WriteLine($"ModifiedRows: {modifiedRows}");
        }

        if (modifiedRows == 0)
        {
            Console.WriteLine($"Transaction annulée");
            // Annule toutes les modifications effectuées dans la transaction et restaure l'état initial
            // de la base de données. Utilisé en cas d'erreur.
            transaction.Rollback();
        }
        else
        {
            Console.WriteLine($"Transaction confirmée");
            // Valide définitivement toutes les modifications effectuées dans la transaction.
            // Les changements deviennent permanents dans la base de données.
            transaction.Commit();
        }
    }
}