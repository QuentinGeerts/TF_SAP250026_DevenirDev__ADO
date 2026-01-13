/*
 * Exercice Page 86
 * ◼ Instanciez un objet de type « Student » contenant vos informations
 * ◼ Insérez votre objet en base de données en récupérant son « ID » au passage
 */

using ExercicePage86.Models;
using Microsoft.Data.SqlClient;

Student quentin = new Student("Geerts", "Quentin", new DateTime(1996, 4, 3), 12, 1010);


string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ExerciceADO;Integrated Security=True;Trust Server Certificate=True";

using (SqlConnection connection = new SqlConnection(connectionString))
{
    using (SqlCommand cmd = connection.CreateCommand())
    {
        cmd.CommandText = $"INSERT INTO Student (FirstName, LastName, BirthDate, YearResult, SectionId) " +
            $"OUTPUT inserted.Id " +
            $"VALUES ('{quentin.Firstname}', '{quentin.Lastname}', '{quentin.BirthDate.ToString("yyyy-MM-dd")}', " +
            $"{(object)quentin.YearResult ?? "NULL"}, {quentin.SectionId})";

        connection.Open();
        int id = (int)cmd.ExecuteScalar();
        connection.Close();

        Console.WriteLine($"Étudiant inséré avec succès, id={id}");
    }
}

