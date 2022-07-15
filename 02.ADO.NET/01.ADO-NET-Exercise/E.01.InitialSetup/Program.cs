using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace E._01.InitialSetup
{
    internal class Program
    {
        static void Main(string[] args)
        {
<<<<<<< HEAD
            //int villainsId = int.Parse(Console.ReadLine()); -> input for 3.Minion Names
=======
            // Minion: Bob 14 Berlin
            // Villain: Gru
            string[] minionData = Console.ReadLine()
                .Split(": ", StringSplitOptions.RemoveEmptyEntries)[1]
                .Split(" ",StringSplitOptions.RemoveEmptyEntries)
                .ToArray();
            string villianName = Console.ReadLine()
                .Split(": ", StringSplitOptions.RemoveEmptyEntries)[1];
            string minionName = minionData[0];
            int minionAge = int.Parse(minionData[1]);
            string minionTown = minionData[2];
            
>>>>>>> 4882b8a6fcabc22921713999f8a795f48a4addcd

            using SqlConnection sqlConnection = new SqlConnection(Config.ConnectionString);

            sqlConnection.Open();
            //string result = GetVillainsWithMinionsCount(sqlConnection); -> output for 2.Villain Names
            //string result = GetAllMinionsNamePerVillain(sqlConnection, villainsId); -> output for 3.Minion Names

            Console.WriteLine(result);

            sqlConnection.Close();
        }


        // 2.Villain Names

        /// <summary>
        /// Take an open sql connection, connects to the database and returns all villains with muinions
        /// </summary>
        /// <param name="sqlConnection">open Sql Connection</param>
        /// <returns></returns>
        private static string GetVillainsWithMinionsCount(SqlConnection sqlConnection)
        {
            StringBuilder output = new StringBuilder();

            string sqlQuery = @"SELECT [v].[Name],
                                 COUNT([mv].[VillainId]) 
                                    AS [MinionsCount]
                                  FROM [Villains] 
                                    AS [v] 
                                  JOIN [MinionsVillains] 
                                    AS [mv] 
                                    ON [v].[Id] = [mv].[VillainId]
                              GROUP BY [v].[Name]
                          HAVING COUNT([mv].[VillainId]) > 3 
                        ORDER BY COUNT([mv].[VillainId])";

            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
            using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                output.AppendLine($"{sqlDataReader["Name"]} - {sqlDataReader["MinionsCount"]}");
            }

            sqlDataReader.Close();

            return output.ToString().TrimEnd();   
        }


        // 3.Minion Names

        /// <summary>
        /// Find all minions per each vallains
        /// </summary>
        /// <param name="sqlConnection">Open SQL Connection</param>
        /// <param name="villainId">Integer value villains id</param>
        /// <returns>String as result</returns>
        private static string GetAllMinionsNamePerVillain(SqlConnection sqlConnection, int villainId)
        {
            StringBuilder output = new StringBuilder();

            string sqlQuery = @" SELECT [Name] 
                                   FROM [Villains] 
                                  WHERE [Id] = @VillainId";

            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);

            // To avoid SQL indjection
            sqlCommand.Parameters.AddWithValue("@VillainId", villainId);

            string villainName = (string)sqlCommand.ExecuteScalar();

            if (villainName == null)
            {
                return $"No villain with ID {villainId} exists in the database.";
            }

            output.AppendLine($"Villain: {villainName}");

            string minionsQuery = @"SELECT [m].[Name],
                                           [m].[Age]
                                      FROM [MinionsVillains]
                                        AS [mv]
                                 LEFT JOIN [Minions]
                                        AS [m]
                                        ON [m].[Id] = [mv].[MinionId]
                                     WHERE [mv].[VillainId] = @VillainId
                                  ORDER BY [m].[Name]";

            SqlCommand sqlCmdGetMinions = new SqlCommand(minionsQuery, sqlConnection);

            // To avoid SQL indjection
            sqlCmdGetMinions.Parameters.AddWithValue("@VillainId", villainId);

            using SqlDataReader minionsReader = sqlCmdGetMinions.ExecuteReader();
            if (!minionsReader.HasRows)
            {
                output.AppendLine($"(no minions)");
            }
            else
            {
                int rowNumber = 1;
                while (minionsReader.Read())
                {
                    output.AppendLine($"{rowNumber}. {minionsReader["Name"]} {minionsReader["Age"]}");
                    rowNumber++;
                }
            }
            
            minionsReader.Close();

            return output.ToString().TrimEnd();
        }

        // 4.Add Minion
<<<<<<< HEAD

        private static string AddMinions()
        {

=======
        private static string AddNewMinion(SqlConnection sqlConnection,
            string[] minionInfo, string villainName)
        {
            StringBuilder output = new StringBuilder();

            string minionName = minionInfo[0];
            int minionAge = int.Parse(minionInfo[1]);
            string townName = minionInfo[2];

            SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
            try
            {
                int townId = GetTownId(sqlConnection, sqlTransaction, output, townName);
                int villainId = GetVillainId(sqlConnection, sqlTransaction, output, villainName);
                int minionId = AddMinionAndGetId(sqlConnection, sqlTransaction, minionName, minionAge, townId);

                string addMinionToVillainQuery = @"INSERT INTO [MinionsVillains]([MinionId], [VillainId])
                                                        VALUES
                                                        (@MinionId, @VillainId)";
                SqlCommand addMinionToVillainCmd =
                    new SqlCommand(addMinionToVillainQuery, sqlConnection, sqlTransaction);
                addMinionToVillainCmd.Parameters.AddWithValue("@MinionId", minionId);
                addMinionToVillainCmd.Parameters.AddWithValue("@VillainId", villainId);

                addMinionToVillainCmd.ExecuteNonQuery();
                output.AppendLine($"Successfully added {minionName} to be minion of {villainName}.");

                sqlTransaction.Commit();
            }
            catch (Exception e)
            {
                sqlTransaction.Rollback();
                return e.ToString();
            }

            return output.ToString().TrimEnd();
        }

        private static int GetTownId(SqlConnection sqlConnection, SqlTransaction sqlTransaction, StringBuilder output, string townName)
        {
            string townIdQuery = @"SELECT [Id]
                                     FROM [Towns]
                                    WHERE [Name] = @TownName";
            SqlCommand townIdCmd = new SqlCommand(townIdQuery, sqlConnection, sqlTransaction);
            townIdCmd.Parameters.AddWithValue("@TownName", townName);

            object townIdObj = townIdCmd.ExecuteScalar();
            if (townIdObj == null)
            {
                string addTownQuery = @"INSERT INTO [Towns]([Name])
                                             VALUES
                                                    (@TownName)";
                SqlCommand addTownCmd = new SqlCommand(addTownQuery, sqlConnection, sqlTransaction);
                addTownCmd.Parameters.AddWithValue("@TownName", townName);

                addTownCmd.ExecuteNonQuery();

                output.AppendLine($"Town {townName} was added to the database.");

                townIdObj = townIdCmd.ExecuteScalar();
            }

            return (int)townIdObj;
        }

        private static int GetVillainId(SqlConnection sqlConnection, SqlTransaction sqlTransaction,
            StringBuilder output, string villainName)
        {
            string villainIdQuery = @"SELECT [Id]
                                          FROM [Villains]
                                         WHERE [Name] = @VillainName";
            SqlCommand villainIdCmd = new SqlCommand(villainIdQuery, sqlConnection, sqlTransaction);
            villainIdCmd.Parameters.AddWithValue("@VillainName", villainName);

            object villainIdObj = villainIdCmd.ExecuteScalar();
            if (villainIdObj == null)
            {
                string evilnessFactorQuery = @"SELECT [Id]
                                                     FROM [EvilnessFactors]
                                                    WHERE [Name] = 'Evil'";
                SqlCommand evilnessFactorCmd =
                    new SqlCommand(evilnessFactorQuery, sqlConnection, sqlTransaction);
                int evilnessFactorId = (int)evilnessFactorCmd.ExecuteScalar();

                string insertVillainQuery = @"INSERT INTO [Villains]([Name], [EvilnessFactorId])
                                                     VALUES
                                                (@VillainName, @EvilnessFactorId)";
                SqlCommand insertVillainCmd =
                    new SqlCommand(insertVillainQuery, sqlConnection, sqlTransaction);
                insertVillainCmd.Parameters.AddWithValue("@VillainName", villainName);
                insertVillainCmd.Parameters.AddWithValue("@EvilnessFactorId", evilnessFactorId);

                insertVillainCmd.ExecuteNonQuery();
                output.AppendLine($"Villain {villainName} was added to the database.");

                villainIdObj = villainIdCmd.ExecuteScalar();
            }

            return (int)villainIdObj;
        }

        private static int AddMinionAndGetId(SqlConnection sqlConnection, SqlTransaction sqlTransaction,
            string minionName, int minionAge, int townId)
        {
            string addMinionQuery = @"INSERT INTO [Minions]([Name], [Age], [TownId])
                                               VALUES 
                                            (@MinionName, @MinionAge, @TownId)";
            SqlCommand addMinionCmd = new SqlCommand(addMinionQuery, sqlConnection, sqlTransaction);
            addMinionCmd.Parameters.AddWithValue("@MinionName", minionName);
            addMinionCmd.Parameters.AddWithValue("@MinionAge", minionAge);
            addMinionCmd.Parameters.AddWithValue("@TownId", townId);

            addMinionCmd.ExecuteNonQuery();

            string addedMinionIdQuery = @"SELECT [Id]
                                       FROM [Minions]
                                      WHERE [Name] = @MinionName AND [Age] = @MinionAge AND [TownId] = @TownId";
            SqlCommand getMinionIdCmd = new SqlCommand(addedMinionIdQuery, sqlConnection, sqlTransaction);
            getMinionIdCmd.Parameters.AddWithValue("@MinionName", minionName);
            getMinionIdCmd.Parameters.AddWithValue("@MinionAge", minionAge);
            getMinionIdCmd.Parameters.AddWithValue("@TownId", townId);

            int minionId = (int)getMinionIdCmd.ExecuteScalar();

            return minionId;
        }


        // 6.*Remove Villain

        private static string DeleteVillain(SqlConnection sqlConnection, int villainId)
        {
            StringBuilder output = new StringBuilder();

            string villainNameQuery = @"SELECT [Name]
                                          FROM [Villains]
                                         WHERE [Id] = @VillainId";
            SqlCommand villainNameCmd = new SqlCommand(villainNameQuery, sqlConnection);
            villainNameCmd.Parameters.AddWithValue("@VillainId", villainId);

            string villainName = (string)villainNameCmd.ExecuteScalar();
            if (villainName == null)
            {
                return $"No such villain was found.";
            }

            SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
            try
            {
                string releaseMinionsQuery = @"DELETE FROM [MinionsVillains]
                                                 WHERE [VillainId] = @VillainId";
                SqlCommand releaseMinionsCmd =
                    new SqlCommand(releaseMinionsQuery, sqlConnection, sqlTransaction);
                releaseMinionsCmd.Parameters.AddWithValue("@VillainId", villainId);

                int minionsReleased = releaseMinionsCmd.ExecuteNonQuery();

                string deleteVillainQuery = @"DELETE FROM [Villains]
                                                WHERE [Id] = @VillainId";
                SqlCommand deleteVillainCmd =
                    new SqlCommand(deleteVillainQuery, sqlConnection, sqlTransaction);
                deleteVillainCmd.Parameters.AddWithValue("@VillainId", villainId);

                int villainsDeleted = deleteVillainCmd.ExecuteNonQuery();

                if (villainsDeleted != 1)
                {
                    sqlTransaction.Rollback();
                }

                output
                    .AppendLine($"{villainName} was deleted.")
                    .AppendLine($"{minionsReleased} minions were released.");
            }
            catch (Exception e)
            {
                sqlTransaction.Rollback();
                return e.ToString();
            }

            sqlTransaction.Commit();

            return output.ToString().TrimEnd();
        }


        // 9.Increase Age Stored Procedure 
        private static string IncreaseMinionAge(SqlConnection sqlConnection, int minionId)
        {
            StringBuilder output = new StringBuilder();

            string increaseAgeQuery = @"EXEC [dbo].[usp_GetOlder] @MinionId";
            SqlCommand increaseAgeCmd = new SqlCommand(increaseAgeQuery, sqlConnection);
            increaseAgeCmd.Parameters.AddWithValue("@MinionId", minionId);

            increaseAgeCmd.ExecuteNonQuery();

            string minionInfoQuery = @"SELECT [Name],
                                              [Age]
                                         FROM [Minions]
                                        WHERE [Id] = @MinionId";
            SqlCommand minionInfoCmd = new SqlCommand(minionInfoQuery, sqlConnection);
            minionInfoCmd.Parameters.AddWithValue("@MinionId", minionId);

            using SqlDataReader infoReader = minionInfoCmd.ExecuteReader();
            while (infoReader.Read())
            {
                output.AppendLine($"{infoReader["Name"]} – {infoReader["Age"]} years old");
            }

            return output.ToString().TrimEnd();
>>>>>>> 4882b8a6fcabc22921713999f8a795f48a4addcd
        }
    }

    
}
