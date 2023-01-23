namespace ADONET_Exercise
{
    using System.Data.SqlClient;
    using System.Text;
    using ADO.NET_Exercise;
    using ADO.NET_Exercise.IO;
    using ADO.NET_Exercise.IO.Contracts;

    public class StartUp
    {
        private IWriter writer;
        private IReader reader;

        public StartUp()
        {
            this.writer = new FileWriter();
            this.reader = new Reader();
        }
        static void Main(string[] args)
        {
            //create connection 
            using SqlConnection sqlConnection =
                        new SqlConnection(Config.ConnectionString);

            //open the connection
            sqlConnection.Open();

            string result = GetVillainNamesWithMinionsCount(sqlConnection);
            FileWriter writer = new FileWriter();

            writer.WriteLine(result);

            //close the connection
            sqlConnection.Close();
            writer.WriteLine("Connection check: Connected!");
        }

        private static string GetVillainNamesWithMinionsCount(SqlConnection sqlConnection)
        {
            StringBuilder sb = new StringBuilder();

            string query = @"SELECT	
                                    v.[Name],
                                    COUNT(mv.MinionId) AS MinionsCount
                            FROM Villains AS v
                             JOIN MinionsVillains AS mv ON mv.VillainId = v.Id
                            GROUP BY v.[Name]
                            HAVING COUNT(mv.MinionId) > 3
                            ORDER BY COUNT(mv.MinionId)  DESC";
                            

            SqlCommand sqlCmd = new SqlCommand(query, sqlConnection);

            using SqlDataReader reader = sqlCmd.ExecuteReader();

            while (reader.Read())
            {
                sb.AppendLine($"{reader["Name"]} - {reader["MinionsCount"]}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}