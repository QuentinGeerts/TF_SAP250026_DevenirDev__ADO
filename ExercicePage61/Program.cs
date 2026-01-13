/*
 *  Exercice Page 61
 *  Établissez la connexion à votre base de données « ADO »
 */

using Microsoft.Data.SqlClient;

string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ExerciceADO;Integrated Security=True;Trust Server Certificate=True";

using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    Console.WriteLine($"Connexion établie");
}
