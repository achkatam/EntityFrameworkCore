namespace _3.MinionNames
{
    using System.Data.SqlClient;
    using System.Text;
    using ADO.NET_Exercise.IO;
    using ADO.NET_Exercise.IO.Contracts;

    public class StartUp
    { 
        static void Main()
        {
            int villainId = int.Parse(Console.ReadLine());

            using SqlConnection sqlConnection =
                        new SqlConnection(Config.ConnectionString);


            sqlConnection.Open();

            string result = GetMinionsNames(sqlConnection, villainId);

            Console.WriteLine(result);

            sqlConnection.Close();

            //if everything is fine the following text would be printed
            Console.WriteLine("Connection check: Connected!");
        }

        private static string GetMinionsNames(SqlConnection sqlConnection, int villainId)
        {
            StringBuilder sb = new StringBuilder();

            string query =
                    @"SELECT [Name] 
                    FROM Villains 
                    WHERE Id = @Id";

            SqlCommand sqlCmd = new SqlCommand(query, sqlConnection);

            sqlCmd.Parameters.AddWithValue("@Id", villainId);

            string villainName = (string)sqlCmd.ExecuteScalar();

            if (villainName == null)
            {
                return

                 ($"No villain with ID {villainId} exists in the database.");
            }

            sb.AppendLine($"Villain: {villainName}");

            string minionNameQuery = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                              m.Name, 
                                              m.Age
                                         FROM MinionsVillains AS mv
                                         JOIN Minions As m ON mv.MinionId = m.Id
                                        WHERE mv.VillainId = @Id
                                     ORDER BY m.Name";

            SqlCommand minionCommand = new SqlCommand(minionNameQuery, sqlConnection);

            minionCommand.Parameters.AddWithValue(@"Id", villainId);

            using SqlDataReader minionsReader = minionCommand.ExecuteReader();
            
            if (!minionsReader.HasRows)
            {
                sb.AppendLine($"(no minions)");

                return sb.ToString().TrimEnd();
            }
            else
            {
                int cnt = 1;

                while (minionsReader.Read())
                {
                    sb.AppendLine($"{cnt}. {minionsReader["Name"]} {minionsReader["Age"]}");
                    cnt++;
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}