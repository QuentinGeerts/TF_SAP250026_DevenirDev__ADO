# SAP250026 - Devenir Développeur - ADO.NET

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120?logo=csharp)](https://docs.microsoft.com/dotnet/csharp/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-LocalDB-CC2927?logo=microsoft-sql-server)](https://www.microsoft.com/sql-server)
[![License](https://img.shields.io/badge/License-Educational-blue.svg)]()

Ce repository contient l'ensemble des démonstrations et exercices du cours ADO.NET. Chaque projet illustre les concepts fondamentaux de l'accès aux données avec des exemples pratiques et progressifs couvrant les modes connecté et déconnecté, les requêtes paramétrées, les procédures stockées et la gestion des transactions.

## 📚 Table des Matières

- [Structure du Projet](#-structure-du-projet)
- [Bases de Données](#-bases-de-données)
- [Démonstrations](#-démonstrations)
- [Exercices Pratiques](#-exercices-pratiques)
- [Technologies](#-technologies-utilisées)
- [Installation](#-installation)
- [Guide d'Apprentissage](#-guide-dapprentissage)

---

## 📂 Structure du Projet

```
SAP250026_DevenirDev__ADO/
├── DemoADO/
│   ├── Models/
│   │   └── User.cs
│   ├── Program.cs
│   └── DemoADO.csproj
├── DemoADO.Database/
│   ├── Tables/
│   │   ├── User.sql
│   │   └── Todo.sql
│   ├── Views/
│   │   └── V_User.sql
│   ├── StoredProcedures/
│   │   ├── SP_User_AddUser.sql
│   │   └── SP_Todo_AddTodo.sql
│   └── Scripts/
│       └── Script.PostDeployment1.sql
├── ExerciceADO/
│   ├── Tables/
│   │   ├── Section.sql
│   │   └── Student.sql
│   ├── Views/
│   │   └── V_Student.sql
│   ├── StoredProcedures/
│   │   ├── SP_Student_AddStudent.sql
│   │   ├── SP_Student_UpdateStudent.sql
│   │   └── SP_Student_DeleteStudent.sql
│   └── Triggers/
│       └── TR_Student_Active.sql
├── ExercicePage61/
├── ExercicePage78/
└── ExercicePage86/
```

---

## 🗄️ Bases de Données

### DemoADO - Base de Démonstration

**Tables:**

**User**
```sql
CREATE TABLE [dbo].[User]
(
    [Id] INT IDENTITY PRIMARY KEY,
    [Email] NVARCHAR(100) NOT NULL,
    [Password] NVARCHAR(255) NOT NULL,
    [Lastname] NVARCHAR(50) NULL,
    [Firstname] NVARCHAR(50) NULL,
    [CreatedAt] DATETIME2 DEFAULT GETDATE(),
    [UpdatedAt] DATETIME2 DEFAULT GETDATE(),
    [IsActive] BIT DEFAULT 1,
    CONSTRAINT [CK_User_Email] CHECK (Email LIKE '%__@%__.%__')
)
```

**Todo**
```sql
CREATE TABLE [dbo].[Todo]
(
    [Id] INT IDENTITY PRIMARY KEY,
    [Title] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(MAX) NULL,
    [Status] NVARCHAR(20) NOT NULL,
    [CreatedAt] DATETIME2 DEFAULT GETDATE(),
    [UpdatedAt] DATETIME2 DEFAULT GETDATE(),
    [IsActive] BIT DEFAULT 1,
    [UserId] INT NOT NULL,
    CONSTRAINT [FK_Todo_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id])
)
```

**Vue V_User**
```sql
CREATE VIEW [dbo].[V_User] AS 
SELECT [Id], [Email], [Lastname], [Firstname] 
FROM [User]
WHERE [IsActive] = 1
```

**Triggers:**
- `TR_User_UpdatedAt` - Met à jour automatiquement le champ UpdatedAt
- `TR_User_IsActive` - Soft-delete (désactivation au lieu de suppression)
- `TR_Todo_UpdatedAt` - Met à jour automatiquement le champ UpdatedAt
- `TR_Todo_IsActive` - Soft-delete pour les todos

---

### ExerciceADO - Base d'Exercices

**Tables:**

**Section**
```sql
CREATE TABLE [dbo].[Section]
(
    [Id] INT NOT NULL PRIMARY KEY,
    [SectionName] VARCHAR(50) NOT NULL
)
```

**Student**
```sql
CREATE TABLE [dbo].[Student]
(
    [Id] INT PRIMARY KEY IDENTITY,
    [FirstName] VARCHAR(50) NOT NULL,
    [LastName] VARCHAR(50) NOT NULL,
    [BirthDate] DATETIME2 NOT NULL,
    [YearResult] INT NULL,
    [SectionId] INT NOT NULL,
    [Active] BIT DEFAULT 1,
    CONSTRAINT [FK_Student_Section] FOREIGN KEY ([SectionId]) REFERENCES [Section]([Id]),
    CONSTRAINT [CK_Student_YearResult] CHECK ([YearResult] BETWEEN 0 AND 20),
    CONSTRAINT [CK_Student_BirthDate] CHECK ([BirthDate] >= '1930-01-01')
)
```

**Données de test:** 25 étudiants répartis dans 6 sections

---

## 🎓 Démonstrations

### 01 - Projet Base de Données SQL Server

**Concepts abordés:**
- Création d'un projet SQL Server
- Création des tables avec contraintes
- Création de vues
- Création de procédures stockées
- Scripts de déploiement post-deployment

**Structure recommandée:**
```
MonProjet.Database/
├── Tables/          # Définitions des tables
├── Views/           # Vues SQL
├── StoredProcedures/ # Procédures stockées
├── Triggers/        # Déclencheurs
└── Scripts/         # Scripts de données initiales
```

---

### 02 - Espace de Noms ADO.NET

**Package NuGet:** `Microsoft.Data.SqlClient`

**Installation:**
```bash
dotnet add package Microsoft.Data.SqlClient
```

**Ou via Visual Studio:**
1. Clic droit sur le projet → Manage NuGet Packages
2. Browse → Rechercher "Microsoft.Data.SqlClient"
3. Install

```csharp
using Microsoft.Data.SqlClient;
using System.Data;
```

---

### 03 - Notion de Connexion

#### 3.1 - Connection String

**Définition:** Chaîne de connexion contenant les paramètres nécessaires pour se connecter à une base de données.

**Récupération via Visual Studio:**
1. Ouvrir Server Explorer (Ctrl + Alt + S)
2. Data Connections → Add Connection
3. Data Source: Microsoft SQL Server
4. Entrer le nom du serveur + Trust Server Certificate
5. Sélectionner la base de données
6. Clic droit → Properties (F4)
7. Copier le champ "Connection String"

```csharp
string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;
                           Initial Catalog=DemoADO;
                           Integrated Security=True;
                           Encrypt=True;
                           Trust Server Certificate=True";
```

**Composants principaux:**
- `Data Source` - Nom du serveur
- `Initial Catalog` - Nom de la base de données
- `Integrated Security` - Authentification Windows
- `Trust Server Certificate` - Accepter le certificat SSL

---

#### 3.2 - Classe SqlConnection

**Définition:** Représente une connexion ouverte à une base de données SQL Server.

**Propriétés importantes:**
- `State` - État de la connexion (Closed, Open, Connecting, etc.)
- `DataSource` - Nom du serveur
- `Database` - Nom de la base de données

**Méthodes:**
- `Open()` - Ouvre la connexion
- `Close()` - Ferme la connexion
- `Dispose()` - Libère les ressources

```csharp
SqlConnection connection = new SqlConnection(connectionString);

Console.WriteLine($"State: {connection.State}"); // Closed
connection.Open();
Console.WriteLine($"State: {connection.State}"); // Open
connection.Close();
Console.WriteLine($"State: {connection.State}"); // Closed
```

⚠️ **Problème:** Si une exception survient entre `Open()` et `Close()`, la connexion reste bloquée dans le pool !

---

#### 3.3 - Instruction using

**Solution:** Le bloc `using` garantit la fermeture automatique de la connexion via `Dispose()`.

```csharp
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    Console.WriteLine($"State: {connection.State}");
    Console.WriteLine($"Data Source: {connection.DataSource}");
    Console.WriteLine($"Database: {connection.Database}");
    
    // Même en cas d'exception, Dispose() est appelé automatiquement
    // La connexion retourne dans le pool
}
```

**Avantages:**
- ✅ Fermeture automatique garantie
- ✅ Gestion correcte des exceptions
- ✅ Retour dans le pool de connexions
- ✅ Libération des ressources

---

### 04 - Classe SqlCommand

**Définition:** Représente une requête SQL ou une procédure stockée à exécuter.

**4 façons de créer un SqlCommand:**

```csharp
using (SqlConnection connection = new SqlConnection(connectionString))
{
    // a. Constructeur vide
    using (SqlCommand command = new SqlCommand())
    {
        command.CommandText = "SELECT * FROM Student";
        command.Connection = connection;
    }
    
    // b. Avec la commande
    using (SqlCommand command = new SqlCommand("SELECT * FROM Student"))
    {
        command.Connection = connection;
    }
    
    // c. Avec commande et connexion
    using (SqlCommand command = new SqlCommand("SELECT * FROM Student", connection))
    {
        // Prêt à l'emploi
    }
    
    // d. Via CreateCommand() (✅ Recommandé)
    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "SELECT * FROM Student";
    }
}
```

---

### 05 - Mode "Connecté"

**Définition:** Maintient la connexion ouverte pendant la lecture/manipulation des données.

**Caractéristiques:**
- ✅ Rapide et performant
- ✅ Lecture séquentielle
- ⚠️ Bloque la connexion pendant l'utilisation
- ⚠️ Lecture seule (forward-only)

**Utilisation:** Lecture séquentielle, grandes quantités de données, opérations rapides.

---

#### 5.1 - Méthode ExecuteScalar

**Définition:** Exécute la requête et retourne la première colonne de la première ligne.

**Utilisation:** Récupérer une valeur unique (COUNT, MAX, ID, etc.)

```csharp
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
```

**Exemples d'utilisation:**
```csharp
// Compter les lignes
command.CommandText = "SELECT COUNT(*) FROM Student";
int count = (int)command.ExecuteScalar();

// Récupérer un ID maximum
command.CommandText = "SELECT MAX(Id) FROM User";
int maxId = (int)command.ExecuteScalar();

// Vérifier l'existence
command.CommandText = "SELECT COUNT(*) FROM User WHERE Email = 'test@test.com'";
bool exists = (int)command.ExecuteScalar() > 0;
```

---

#### 5.2 - Méthode ExecuteReader

**Définition:** Exécute la requête et retourne un `SqlDataReader` pour lire les lignes une par une.

**Classe SqlDataReader:**
- Flux de données en lecture seule
- Parcours séquentiel (forward-only)
- Performances optimales
- ⚠️ Doit être fermé après utilisation

**Gestion des NULL:**
- SQL: `NULL`
- C#: `null`
- ADO.NET: `DBNull.Value`

```csharp
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
                // Méthode 1: Par nom de colonne (cast direct)
                int id = (int)reader["Id"];
                
                // Méthode 2: Par index
                string email = (string)reader[1];
                
                // Méthode 3: Opérateur 'as' (retourne null si échec)
                string? lastname = reader["Lastname"] as string;
                
                // Méthode 4: Vérification de DBNull.Value
                string? firstname = reader["Firstname"] is DBNull 
                    ? null 
                    : (string)reader["Firstname"];
                
                Console.WriteLine($"Id: {id}, email: {email}, " +
                                $"lastname: {lastname}, firstname: {firstname}");
            }
        }
        connection.Close();
    }
}
```

**Méthodes typées du SqlDataReader:**
```csharp
int id = reader.GetInt32(0);              // Index 0
string email = reader.GetString(1);       // Index 1
DateTime date = reader.GetDateTime(2);    // Index 2
bool isActive = reader.GetBoolean(3);     // Index 3

// Vérifier si NULL
if (!reader.IsDBNull(4))
{
    string value = reader.GetString(4);
}
```

---

#### 5.3 - Méthode ExecuteNonQuery

**Définition:** Exécute une commande qui ne retourne pas de résultat (INSERT, UPDATE, DELETE, CREATE, etc.).

**Retour:** Nombre de lignes affectées.

```csharp
using (SqlConnection connection = new SqlConnection(connectionString))
{
    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = @"UPDATE [dbo].[User] 
                               SET [Email] = 'quentin.geerts@cognitic.be' 
                               WHERE [Id] = 1";
        
        connection.Open();
        int nbAffectedRows = command.ExecuteNonQuery();
        connection.Close();
        
        Console.WriteLine($"Nombre de lignes affectées: {nbAffectedRows}");
        
        if (nbAffectedRows > 0)
            Console.WriteLine("Modification effectuée.");
        else
            Console.WriteLine("Aucune modification effectuée.");
    }
}
```

**Cas d'utilisation:**
```csharp
// INSERT
command.CommandText = "INSERT INTO User (Email, Password) VALUES ('test@test.com', 'hash')";
int inserted = command.ExecuteNonQuery(); // 1 si succès

// UPDATE
command.CommandText = "UPDATE User SET Email = 'new@email.com' WHERE Id = 1";
int updated = command.ExecuteNonQuery(); // 1 si ligne modifiée

// DELETE
command.CommandText = "DELETE FROM User WHERE Id = 999";
int deleted = command.ExecuteNonQuery(); // 0 si aucune ligne

// DDL (Data Definition Language)
command.CommandText = "CREATE TABLE Test (Id INT PRIMARY KEY)";
int result = command.ExecuteNonQuery(); // -1 pour DDL
```

---

### 06 - Mode "Déconnecté"

**Définition:** Récupère les données en mémoire (DataSet/DataTable), puis ferme la connexion.

**Caractéristiques:**
- ✅ Manipulation offline
- ✅ Modifications multiples
- ✅ Binding UI facilité
- ✅ Mise en cache
- ⚠️ Plus de mémoire utilisée

**Utilisation:** Binding UI, modifications multiples, travail offline, mise en cache.

---

#### 6.1 - Classe SqlDataAdapter

**Définition:** Pont entre une base de données SQL Server et un DataSet/DataTable en mode déconnecté.

**Méthodes principales:**
- `Fill()` - Remplit un DataSet/DataTable (ouverture auto de la connexion)
- `Update()` - Sauvegarde les modifications

```csharp
DataSet dataSet = new DataSet();
DataTable dataTable = new DataTable();

using (SqlConnection connection = new SqlConnection(connectionString))
{
    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "SELECT * FROM [V_User]";
        
        SqlDataAdapter adapter = new SqlDataAdapter(command);
        adapter.Fill(dataSet);    // Remplit le DataSet
        adapter.Fill(dataTable);  // Remplit le DataTable
    } // Fermeture automatique de la connexion
}

// Parcours du DataSet
if (dataSet.Tables.Count > 0)
{
    foreach (DataRow row in dataSet.Tables[0].Rows)
    {
        int id = (int)row["Id"];
        string email = (string)row["Email"];
        Console.WriteLine($"{id} {email}");
    }
}

// Parcours du DataTable
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
```

**Avantages DataSet vs DataTable:**

**DataSet:**
- Collection de DataTables
- Relations entre tables
- Contraintes
- Plus complet mais plus lourd

**DataTable:**
- Table unique
- Plus léger
- Suffisant pour la plupart des cas

---

### 07 - Mot-clé OUTPUT

**Définition:** Clause SQL Server utilisée dans INSERT, UPDATE, DELETE pour retourner les valeurs insérées/modifiées/supprimées.

**Tables virtuelles:**
- `inserted` - Valeurs APRÈS modification (INSERT, UPDATE)
- `deleted` - Valeurs AVANT modification (UPDATE, DELETE)

⚠️ **Distinction importante:**
- `OUTPUT` (dans DML) = clause retournant directement les résultats
- `OUTPUT` (dans procédure) = paramètre de sortie

```csharp
// Récupérer l'ID généré avec ExecuteScalar
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

// Récupérer plusieurs colonnes avec ExecuteReader
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
                string? description = reader["Description"] is DBNull 
                    ? null 
                    : (string)reader["Description"];
                int userId = (int)reader["UserId"];
                
                Console.WriteLine($"{id} {title} {description} {userId}");
            }
        }
        connection.Close();
    }
}
```

**Exemples avec UPDATE et DELETE:**

```csharp
// UPDATE avec OUTPUT
cmd.CommandText = @"
    UPDATE User 
    SET Email = 'new@email.com'
    OUTPUT deleted.Email AS OldEmail, inserted.Email AS NewEmail
    WHERE Id = 1";

// DELETE avec OUTPUT
cmd.CommandText = @"
    DELETE FROM Todo
    OUTPUT deleted.Id, deleted.Title
    WHERE Status = 'Completed'";
```

---

### 08 - Requêtes Paramétrées

**Définition:** Requête SQL utilisant des paramètres (@Param) au lieu de concaténation de chaînes.

**Avantages:**
- ✅ Protection contre les injections SQL
- ✅ Amélioration des performances (plan d'exécution mis en cache)
- ✅ Gestion automatique des types
- ✅ Gestion automatique des caractères spéciaux

❌ **MAUVAIS - Concaténation (injection SQL possible):**
```csharp
string username = "admin' OR '1'='1"; // Injection SQL !
string query = "SELECT * FROM Users WHERE Username = '" + username + "'";
// Résultat: SELECT * FROM Users WHERE Username = 'admin' OR '1'='1'
// => Retourne TOUS les utilisateurs !
```

✅ **BON - Requête paramétrée (sécurisée):**
```csharp
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
                
                Console.WriteLine($"User: {id} {email} {lastname} {firstname}");
            }
        }
        c.Close();
    }
}
```

**Paramètres multiples:**
```csharp
cmd.CommandText = @"SELECT * FROM User 
                   WHERE Email = @Email 
                   AND CreatedAt >= @StartDate";

cmd.Parameters.AddWithValue("Email", "test@test.com");
cmd.Parameters.AddWithValue("StartDate", new DateTime(2024, 1, 1));
```

**Paramètres NULL:**
```csharp
string? lastname = null;

// Mauvais
cmd.Parameters.AddWithValue("Lastname", lastname); // Erreur !

// Bon
cmd.Parameters.AddWithValue("Lastname", (object?)lastname ?? DBNull.Value);
```

**Méthode alternative (plus de contrôle):**
```csharp
cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = 1 });
cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 100) 
{ 
    Value = "test@test.com" 
});
```

---

### 09 - Appel de Procédures Stockées

**Définition:** Exécution d'une procédure stockée SQL Server via SqlCommand.

**Configuration:**
- `CommandText` = Nom de la procédure
- `CommandType` = `CommandType.StoredProcedure`

**Procédure exemple:**
```sql
CREATE PROCEDURE [dbo].[SP_User_AddUser]
    @Email NVARCHAR(100),
    @Password NVARCHAR(255),
    @Lastname NVARCHAR(50) = NULL,
    @Firstname NVARCHAR(50) = NULL,
    @UserId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON
    
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM [dbo].[User] WHERE [Email] = @Email)
            RAISERROR('Email already exists.', 16, 13)
        
        INSERT INTO [dbo].[User] (Email, Password, Lastname, Firstname)
        VALUES (@Email, HASHBYTES('SHA2_256', @Password), @Lastname, @Firstname)
        
        SELECT @UserId = @@IDENTITY FROM [dbo].[User];
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
    
    SET NOCOUNT OFF
END
```

**Appel depuis C#:**
```csharp
using (SqlConnection c = new SqlConnection(connectionString))
{
    using (SqlCommand cmd = c.CreateCommand())
    {
        cmd.CommandText = "SP_User_AddUser";
        cmd.CommandType = CommandType.StoredProcedure;
        
        // Paramètres d'entrée
        string email = "geerts.quentin@gmail.com";
        string password = "Test1234=";
        string? lastname = "Geerts";
        string? firstname = null;
        
        cmd.Parameters.AddWithValue("Email", email);
        cmd.Parameters.AddWithValue("Password", password);
        cmd.Parameters.AddWithValue("Lastname", (object?)lastname ?? DBNull.Value);
        cmd.Parameters.AddWithValue("Firstname", (object?)firstname ?? DBNull.Value);
        
        // Paramètre de sortie (OUTPUT)
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
            Console.WriteLine($"SQL Exception: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erreur inattendue: {e.Message}");
        }
    }
}
```

**Types de paramètres:**

```csharp
// INPUT (par défaut)
cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 100)
{
    Direction = ParameterDirection.Input,
    Value = "test@test.com"
});

// OUTPUT
cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int)
{
    Direction = ParameterDirection.Output
});

// INPUT/OUTPUT
cmd.Parameters.Add(new SqlParameter("@Counter", SqlDbType.Int)
{
    Direction = ParameterDirection.InputOutput,
    Value = 0
});

// RETURN VALUE
cmd.Parameters.Add(new SqlParameter("@ReturnValue", SqlDbType.Int)
{
    Direction = ParameterDirection.ReturnValue
});
```

---

### 10 - Gestion des Transactions

**Définition:** Regroupe plusieurs opérations en une unité atomique. Toutes réussissent (Commit) ou échouent ensemble (Rollback).

**Propriétés ACID:**
- **A**tomicity - Tout ou rien
- **C**onsistency - État cohérent
- **I**solation - Isolation entre transactions
- **D**urability - Persistance

**Utilisation:**
```csharp
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    
    using (SqlTransaction transaction = connection.BeginTransaction())
    {
        int modifiedRows = 0;
        
        try
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction; // ⚠️ Important !
                command.CommandText = "DELETE FROM [dbo].[User] WHERE Id = 2";
                
                modifiedRows = command.ExecuteNonQuery();
                Console.WriteLine($"ModifiedRows: {modifiedRows}");
            }
            
            if (modifiedRows == 0)
            {
                Console.WriteLine("Transaction annulée");
                transaction.Rollback(); // Annule tout
            }
            else
            {
                Console.WriteLine("Transaction confirmée");
                transaction.Commit(); // Valide tout
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur: {ex.Message}");
            transaction.Rollback(); // Annule en cas d'erreur
        }
    }
}
```

**Exemple pratique - Transfert bancaire:**
```csharp
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    using (SqlTransaction transaction = connection.BeginTransaction())
    {
        try
        {
            // Débiter le compte source
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.Transaction = transaction;
                cmd.CommandText = "UPDATE Account SET Balance = Balance - @Amount WHERE Id = @Id";
                cmd.Parameters.AddWithValue("Amount", 100);
                cmd.Parameters.AddWithValue("Id", 1);
                cmd.ExecuteNonQuery();
            }
            
            // Créditer le compte destination
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.Transaction = transaction;
                cmd.CommandText = "UPDATE Account SET Balance = Balance + @Amount WHERE Id = @Id";
                cmd.Parameters.AddWithValue("Amount", 100);
                cmd.Parameters.AddWithValue("Id", 2);
                cmd.ExecuteNonQuery();
            }
            
            // Tout s'est bien passé
            transaction.Commit();
            Console.WriteLine("Transfert réussi");
        }
        catch (Exception ex)
        {
            // Une erreur s'est produite, annuler tout
            transaction.Rollback();
            Console.WriteLine($"Transfert échoué: {ex.Message}");
        }
    }
}
```

**Points de sauvegarde (Savepoints):**
```csharp
using (SqlTransaction transaction = connection.BeginTransaction())
{
    try
    {
        // Opération 1
        cmd.ExecuteNonQuery();
        
        // Créer un point de sauvegarde
        transaction.Save("SavePoint1");
        
        // Opération 2 (peut échouer)
        cmd.ExecuteNonQuery();
    }
    catch
    {
        // Revenir au point de sauvegarde
        transaction.Rollback("SavePoint1");
    }
    
    transaction.Commit();
}
```

---

## 🏋️ Exercices Pratiques

### Exercice Page 61 - Connexion à la Base

**Objectif:** Établir une connexion à la base de données ExerciceADO.

```csharp
string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;
                           Initial Catalog=ExerciceADO;
                           Integrated Security=True;
                           Trust Server Certificate=True";

using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    Console.WriteLine("Connexion établie");
}
```

**Vérifications:**
- ✅ Connexion ouverte sans erreur
- ✅ Pas d'exception levée
- ✅ Message de confirmation affiché

---

### Exercice Page 78 - Lectures Connectée et Déconnectée

**Objectif:** Pratiquer les deux modes d'accès aux données.

**Partie 1 - Mode Connecté:**
Afficher l'ID, Nom et Prénom de chaque étudiant depuis V_Student.

```csharp
using (SqlConnection connection = new SqlConnection(connectionString))
{
    using (SqlCommand command = connection.CreateCommand())
    {
        command.CommandText = "SELECT [Id], [LastName], [FirstName] FROM [dbo].[V_Student]";
        
        connection.Open();
        using (SqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                int id = (int)reader["Id"];
                string lastname = (string)reader["LastName"];
                string firstname = (string)reader["FirstName"];
                
                Console.WriteLine($"{id} {lastname} {firstname}");
            }
        }
        connection.Close();
    }
}
```

**Partie 2 - Mode Déconnecté:**
Afficher l'ID et le Nom de chaque section.

```csharp
DataTable dt = new DataTable();

using (SqlConnection connection = new SqlConnection(connectionString))
{
    using (SqlCommand cmd = connection.CreateCommand())
    {
        cmd.CommandText = "SELECT * FROM [dbo].[Section]";
        
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        adapter.Fill(dt);
    }
}

if (dt.Rows.Count > 0)
{
    foreach (DataRow row in dt.Rows)
    {
        int id = (int)row["Id"];
        string sectionName = (string)row["SectionName"];
        
        Console.WriteLine($"{id} {sectionName}");
    }
}
else
{
    Console.WriteLine("Aucune donnée à afficher");
}
```

**Partie 3 - ExecuteScalar:**
Afficher la moyenne annuelle des étudiants.

```csharp
using (SqlConnection connection = new SqlConnection(connectionString))
{
    using (SqlCommand cmd = connection.CreateCommand())
    {
        cmd.CommandText = "SELECT AVG(CONVERT(FLOAT, YearResult)) FROM V_Student";
        
        connection.Open();
        double moyenne = (double)cmd.ExecuteScalar();
        connection.Close();
        
        Console.WriteLine($"Moyenne des élèves: {moyenne:F2}");
    }
}
```

---

### Exercice Page 86 - Insertion avec OUTPUT

**Objectif:** Insérer un étudiant et récupérer son ID généré.

**Modèle Student:**
```csharp
internal class Student
{
    public string Lastname { get; set; }
    public string Firstname { get; set; }
    public DateTime BirthDate { get; set; }
    public int? YearResult { get; set; }
    public int SectionId { get; set; }
    
    public Student(string lastname, string firstname, DateTime birthDate, 
                   int? yearResult, int sectionId)
    {
        Lastname = lastname;
        Firstname = firstname;
        BirthDate = birthDate;
        YearResult = yearResult;
        SectionId = sectionId;
    }
}
```

**Insertion:**
```csharp
Student quentin = new Student("Geerts", "Quentin", 
                             new DateTime(1996, 4, 3), 12, 1010);

using (SqlConnection connection = new SqlConnection(connectionString))
{
    using (SqlCommand cmd = connection.CreateCommand())
    {
        cmd.CommandText = @"
            INSERT INTO Student (FirstName, LastName, BirthDate, YearResult, SectionId)
            OUTPUT inserted.Id
            VALUES (@Firstname, @Lastname, @BirthDate, @YearResult, @SectionId)";
        
        cmd.Parameters.AddWithValue("Firstname", quentin.Firstname);
        cmd.Parameters.AddWithValue("Lastname", quentin.Lastname);
        cmd.Parameters.AddWithValue("BirthDate", quentin.BirthDate);
        cmd.Parameters.AddWithValue("YearResult", (object?)quentin.YearResult ?? DBNull.Value);
        cmd.Parameters.AddWithValue("SectionId", quentin.SectionId);
        
        connection.Open();
        int id = (int)cmd.ExecuteScalar();
        connection.Close();
        
        Console.WriteLine($"Étudiant inséré avec succès, id={id}");
    }
}
```

⚠️ **Version avec concaténation (À NE PAS FAIRE):**
```csharp
// ❌ Injection SQL possible !
cmd.CommandText = $"INSERT INTO Student (FirstName, LastName, BirthDate, YearResult, SectionId) " +
    $"OUTPUT inserted.Id " +
    $"VALUES ('{quentin.Firstname}', '{quentin.Lastname}', '{quentin.BirthDate:yyyy-MM-dd}', " +
    $"{(object)quentin.YearResult ?? "NULL"}, {quentin.SectionId})";
```

✅ **Version sécurisée avec paramètres (À FAIRE):**
```csharp
// Version de l'exercice ci-dessus
```

---

## 🛠️ Technologies Utilisées

- **.NET 10.0** - Framework de développement
- **C# 12** - Langage de programmation
- **ADO.NET** - Technologie d'accès aux données
- **Microsoft.Data.SqlClient 6.1.3** - Provider SQL Server
- **SQL Server LocalDB** - Base de données locale
- **Visual Studio 2025** - IDE recommandé

### Composants ADO.NET

| Classe | Description | Usage |
|--------|-------------|-------|
| SqlConnection | Gère la connexion | Toujours avec `using` |
| SqlCommand | Exécute les requêtes | ExecuteScalar, ExecuteReader, ExecuteNonQuery |
| SqlDataReader | Lecture forward-only | Mode connecté, performant |
| SqlDataAdapter | Remplit DataSet/DataTable | Mode déconnecté |
| SqlTransaction | Gère les transactions | Commit/Rollback |
| SqlParameter | Paramètre sécurisé | Protection injection SQL |

---

## 📥 Installation

### Prérequis

1. **[.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)**
2. **[Visual Studio 2025](https://visualstudio.microsoft.com/)** avec:
   - Développement .NET Desktop
   - Développement ASP.NET et web
   - Stockage et traitement de données
   - SQL Server Data Tools (SSDT)
3. **SQL Server LocalDB** (inclus avec Visual Studio)

### Cloner le Projet

```bash
git clone https://github.com/votre-username/SAP250026_DevenirDev__ADO.git
cd SAP250026_DevenirDev__ADO
```

### Ouvrir la Solution

```bash
# Avec Visual Studio
start SAP250026_DevenirDev__ADO.sln

# Avec VS Code
code .
```

### Déployer les Bases de Données

####  Via Projet Database

1. Clic droit sur **DemoADO.Database** → **Publish**
2. **Edit Connection** → Serveur: `(localdb)\MSSQLLocalDB`
3. Database name: `DemoADO`
4. **Publish**
5. Répéter pour **ExerciceADO** → `ExerciceADO`

### Vérifier les Connection Strings

Dans chaque projet, vérifier la connectionString dans `Program.cs`:

```csharp
string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;
                           Initial Catalog=DemoADO;
                           Integrated Security=True;
                           Trust Server Certificate=True";
```

### Exécuter un Projet

```bash
# DemoADO
cd DemoADO
dotnet run

# Exercice
cd ../ExercicePage78
dotnet run

# Ou avec le chemin complet
dotnet run --project DemoADO/DemoADO.csproj
```

---

## 🎯 Objectifs Pédagogiques

À la fin de ce cours, vous serez capable de:

- ✅ Établir et gérer des connexions SQL Server
- ✅ Exécuter des requêtes en mode connecté et déconnecté
- ✅ Sécuriser les requêtes avec des paramètres
- ✅ Appeler des procédures stockées
- ✅ Gérer les transactions
- ✅ Manipuler SqlDataReader et DataTable
- ✅ Gérer correctement les NULL
- ✅ Choisir la bonne méthode selon le contexte
- ✅ Implémenter des opérations CRUD complètes
- ✅ Respecter les bonnes pratiques ADO.NET

---

## 📚 Ressources Complémentaires

### Documentation Officielle

- [ADO.NET Overview](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/)
- [SqlConnection Class](https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnection)
- [SqlCommand Class](https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlcommand)
- [SqlDataReader Class](https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader)
- [Transactions in ADO.NET](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/local-transactions)

---

## 🔐 Bonnes Pratiques

### Sécurité

✅ **À FAIRE:**
```csharp
// Requêtes paramétrées
cmd.CommandText = "SELECT * FROM User WHERE Id = @Id";
cmd.Parameters.AddWithValue("@Id", userId);

// Hashage des mots de passe
cmd.CommandText = "INSERT INTO User (Password) VALUES (HASHBYTES('SHA2_256', @Password))";
```

❌ **À ÉVITER:**
```csharp
// Concaténation = injection SQL
cmd.CommandText = $"SELECT * FROM User WHERE Id = {userId}";

// Mots de passe en clair
cmd.CommandText = $"INSERT INTO User (Password) VALUES ('{password}')";
```

### Gestion des Ressources

✅ **À FAIRE:**
```csharp
// using garantit Dispose()
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    // ...
} // Fermeture automatique
```

❌ **À ÉVITER:**
```csharp
// Fuite de ressources
SqlConnection connection = new SqlConnection(connectionString);
connection.Open();
// ... Si exception ici, connection jamais fermée !
```

### Gestion des NULL

✅ **À FAIRE:**
```csharp
// Vérification explicite
string? lastname = reader["Lastname"] is DBNull ? null : (string)reader["Lastname"];

// Avec paramètres
cmd.Parameters.AddWithValue("@Lastname", (object?)lastname ?? DBNull.Value);
```

❌ **À ÉVITER:**
```csharp
// Cast direct (crash si NULL)
string lastname = (string)reader["Lastname"];

// NULL non géré
cmd.Parameters.AddWithValue("@Lastname", lastname); // Erreur si null
```

---

## 🐛 Résolution de Problèmes

### Erreur: "Cannot open database"

**Solution:**
1. Vérifier que SQL Server LocalDB est installé
2. Vérifier le nom de la base dans la connection string
3. Déployer la base de données depuis le projet .sqlproj

### Erreur: "Login failed"

**Solution:**
1. Utiliser `Integrated Security=True` pour Windows Authentication
2. Vérifier que votre compte Windows a les droits
3. Ajouter `Trust Server Certificate=True`

### Erreur: "Timeout expired"

**Solution:**
1. Augmenter le timeout: `Connection Timeout=30;`
2. Vérifier que la requête n'est pas trop longue
3. Optimiser les index de la base de données

### SqlDataReader: "Invalid attempt to read when no data is present"

**Solution:**
```csharp
// Vérifier si des données existent
using (SqlDataReader reader = cmd.ExecuteReader())
{
    if (reader.HasRows)
    {
        while (reader.Read())
        {
            // Lecture sécurisée
        }
    }
}
```

## 📝 Licence

Ce projet est sous licence **Educational** - voir le fichier [LICENSE](LICENSE) pour plus de détails.

---

## 👨‍🏫 Formateur

**Quentin Geerts**   
Formation: SAP250026 - Devenir Développeur


---

<div align="center">

**⭐ Bon apprentissage d'ADO.NET ! ⭐**

Made with ❤️ for learning data access in .NET

</div>
