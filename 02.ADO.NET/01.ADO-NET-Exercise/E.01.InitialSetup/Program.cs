using System;
using System.Data.SqlClient;
using System.Text;

namespace E._01.InitialSetup
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            using SqlConnection sqlConnection = new SqlConnection(Config.ConnectionString);
            sqlConnection.Open();
            string result = GetVillainsWithMinionsCount(sqlConnection);
            Console.WriteLine(result);
            sqlConnection.Close();
            
        }

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

    }
}
