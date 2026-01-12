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
using Microsoft.Data.SqlClient;

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

string connectionString = @"Data Source=QUENTIN-BSTORM;Initial Catalog=Preparation;Integrated Security=True;Trust Server Certificate=True";

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

    connection.Close();
}
