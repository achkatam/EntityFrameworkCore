namespace _4.AddMinion
{
    using System;
    using System.Data.SqlClient;
    using System.Text;

    using _4.AddMinion.IO;

    public class StartUp
    {

        //CHECK THE 2ND INPUT
        static void Main(string[] args)
        {
            var writer = new FileWriter();
            var reader = new Reader();


            string minionInfoInput = reader.ReadLine();
            var minionTokens = minionInfoInput.Split(": ", StringSplitOptions.RemoveEmptyEntries);

            var minionInfo = minionTokens[1]
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);


            var villainInfo = reader.ReadLine()
                    .Split(":", StringSplitOptions.RemoveEmptyEntries);
            string villainName = villainInfo[1];

            //Add library from NuGetPackges
            //1. Create the connection
            using SqlConnection sqlConnection =
                        new SqlConnection(Config.ConnectionString);


            //2. Open the connection
            sqlConnection.Open();

            string result = UpdateTable(sqlConnection, writer, reader, minionInfo, villainName);

            writer.WriteLine(result);

            //close the connection
            sqlConnection.Close();

            writer.WriteLine("Connection check: Connected!");
        }

        private static string UpdateTable(SqlConnection sqlConnection, FileWriter writer, Reader reader, string[] minionInfo, string villainName)
        {
            string minionName = minionInfo[0];
            int minionAge = int.Parse(minionInfo[1]);
            string townName = minionInfo[2];

            SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();

            StringBuilder sb = new StringBuilder();

            try
            {
                int townId = GetTownID(sqlConnection, townName, sqlTransaction, sb);
                int villainId = GetVillainId(sqlConnection, sqlTransaction, sb, villainName);
                int minionId = AddMinion(sqlConnection, sqlTransaction, sb, minionName, minionAge, townId);

                string addMinionVillainsQry = @"INSERT INTO MinionsVillains(MinionId, VillainId) 
                                                        VALUES
                                                (@MinionId, @VillainId)";

                SqlCommand addMinionToVillainCmd =
                                new SqlCommand(addMinionVillainsQry, sqlConnection, sqlTransaction);

                addMinionToVillainCmd.Parameters.AddWithValue("@MinionId", minionId);
                addMinionToVillainCmd.Parameters.AddWithValue("@VillainId", villainId);

                addMinionToVillainCmd.ExecuteNonQuery();

                sb.AppendLine($"Successfully added {minionName} to be minion of {villainName}.");
            }
            catch (Exception e)
            {
                sqlTransaction.Rollback();

                return e.ToString();
            }

          
            return sb.ToString().TrimEnd();
        }

        private static int GetTownID(SqlConnection sqlConnection, string townName, SqlTransaction sqlTransaction, StringBuilder sb)
        {
            string townIdQuery = @"SELECT Id
                                        FROM Towns
                                        WHERE [Name] = @townName";

            SqlCommand townIdCmd = new SqlCommand(townIdQuery, sqlConnection, sqlTransaction);

            townIdCmd.Parameters.AddWithValue("@townName", townName);

            object townIdObj = townIdCmd.ExecuteScalar();
     

            if (townIdObj == null)
            {
                string addTownQuery = @"INSERT INTO Towns (Name)
                                               VALUES 
                                            (@townName)";

                SqlCommand addTownCmd = new SqlCommand(addTownQuery, sqlConnection, sqlTransaction);
                addTownCmd.Parameters.AddWithValue("@townName", townName);

                addTownCmd.ExecuteNonQuery();

                sb.AppendLine($"Town {townName} was added to the database.");

                townIdObj = townIdCmd.ExecuteScalar();
            }

            return (int)townIdObj;
        }

        private static int GetVillainId(SqlConnection sqlConnection, SqlTransaction sqlTransaction,StringBuilder sb, string villainName)
        {
            string villainIdQuery = @"SELECT Id
                                            FROM Villains 
                                        WHERE Name = '@villainName'";

            SqlCommand villainIdCmd = new SqlCommand(villainIdQuery, sqlConnection, sqlTransaction);
            villainIdCmd.Parameters.AddWithValue("@villainName", villainName);

            object villainIdObj = villainIdCmd.ExecuteScalar();

            if (villainIdObj == null)
            {

                string evilnessFactorQuery = @"SELECT Id FROM EvilnessFactors WHERE [Name] = 'Evil'";

                SqlCommand evilnessFactorCmd =
                    new SqlCommand(evilnessFactorQuery, sqlConnection, sqlTransaction);

                int evilnessFactorId = (int)evilnessFactorCmd.ExecuteScalar();

                string addVillainQuery = @"INSERT INTO Villains (Name, EvilnessFactorId) 
                                                VALUES 
                                              ('@villainName', @EvilnessFactorId)";

                SqlCommand insertVillainCmd =
                    new SqlCommand(addVillainQuery, sqlConnection, sqlTransaction);

                insertVillainCmd.Parameters.AddWithValue("@villainName", villainName);
                insertVillainCmd.Parameters.AddWithValue("@EvilnessFactorId", evilnessFactorId);

                insertVillainCmd.ExecuteNonQuery();

                sb.AppendLine($"Villain {villainName} was added to the database.");

                villainIdObj = villainIdCmd.ExecuteScalar();
            }

            return (int)villainIdObj;
        }

        private static int AddMinion(SqlConnection sqlConnection, SqlTransaction sqlTransaction, StringBuilder sb, string minionName, int minionAge, int townId)
        {
            string addMinionQuery = @"INSERT INTO Minions (Name, Age, TownId) 
                                                VALUES 
                                             (@MinionName, @MinionAge, @TownId)";
            SqlCommand addMinionCmd = new SqlCommand(addMinionQuery, sqlConnection, sqlTransaction);
            addMinionCmd.Parameters.AddWithValue("@MinionName", minionName);
            addMinionCmd.Parameters.AddWithValue("@MinionAge", minionAge);
            addMinionCmd.Parameters.AddWithValue(@"TownId", townId);

            addMinionCmd.ExecuteNonQuery();

            string addedMinionIdQuery = @"SELECT Id
                                    FROM Minions AS m
                                   WHERE m.Name = @MinionName 
                                    AND m.Age = @MinionAge
                                    AND TownId = @TownId";

            SqlCommand getMinionIdCmd = new SqlCommand(addedMinionIdQuery, sqlConnection, sqlTransaction);
            getMinionIdCmd.Parameters.AddWithValue("@MinionName", minionName);
            getMinionIdCmd.Parameters.AddWithValue("@MinionAge", minionAge);
            getMinionIdCmd.Parameters.AddWithValue(@"TownId", townId);

            int minionId = (int)getMinionIdCmd.ExecuteScalar();

            return minionId;
        }
    }
}
