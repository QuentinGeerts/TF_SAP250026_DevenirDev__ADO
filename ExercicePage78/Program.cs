

using Microsoft.Data.SqlClient;
using System.Data;

string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ExerciceADO;Integrated Security=True;Trust Server Certificate=True";

// Afficher l’« ID », le « Nom », le « Prenom » de chaque étudiant depuis la vue « V_Student » en
// utilisant la méthode connectée

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

// Afficher l’« ID », le « Nom » de chaque section en utilisant la méthode déconnectée

DataTable dt = new DataTable();

using (SqlConnection connection = new SqlConnection(connectionString))
{
    using (SqlCommand cmd = connection.CreateCommand())
    {
        cmd.CommandText = "SELECT * FROM [dbo].[Section]";

        SqlDataAdapter adapter = new SqlDataAdapter(cmd);

        // Ou
        //SqlDataAdapter adapter2 = new SqlDataAdapter();
        //adapter2.SelectCommand = cmd;

        adapter.Fill(dt);
    }
}

if (dt.Rows.Count > 0)
{
    foreach (DataRow row in dt.Rows)
    {
        int id = (int) row["Id"];
        string sectionName = (string)row["SectionName"];

        Console.WriteLine($"{id} {sectionName}");
    }
}
else
{
    Console.WriteLine($"Aucune donnée à afficher");
}

// Afficher la moyenne annuelle des étudiants

using (SqlConnection connection = new SqlConnection(connectionString))
{
    using (SqlCommand cmd = connection.CreateCommand())
    {
        cmd.CommandText = "SELECT AVG(CONVERT(FLOAT, YearResult)) FROM V_Student";

        connection.Open();
        double moyenne = (double)cmd.ExecuteScalar();
        connection.Close();

        Console.WriteLine($"Moyenne des élèves: {moyenne}");
    }
}
