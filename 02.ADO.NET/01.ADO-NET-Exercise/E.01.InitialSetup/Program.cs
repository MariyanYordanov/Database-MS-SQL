using System;
using System.Data.SqlClient;
using System.Text;

namespace E._01.InitialSetup
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //int villainsId = int.Parse(Console.ReadLine()); -> input for 3.Minion Names

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

        private static string AddMinions()
        {

        }
    }
}
