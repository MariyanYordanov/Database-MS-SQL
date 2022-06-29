using System;
using System.Data.SqlClient;


namespace _01.ADO_NET_Exercise
{
    public class StartUp
    {
        static void Main(string[] args)
        {
           // 1.Created connection
            using SqlConnection sqlConnection = new SqlConnection(Config.ConnectionString);

            // 2.Open connection
            sqlConnection.Open();

            // 2.1.Check connection
            Console.WriteLine("Connected!");

            // 3.Create SQL query
            string query = @"SELECT COUNT(*) AS [EmloyeeCount] FROM [Employees]";

            // 4.Create SQL command 

            /* 4.1.Use transaction when write insert/update/delete queries
            SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
            */

            // 4.2.Else
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

            
            int employeeCount = (int)sqlCommand.ExecuteScalar();
            // 4.3. Check command
            Console.WriteLine($" -- Emloyee count: {employeeCount}");

            // 5.And close connection
            sqlConnection.Close();
            sqlConnection.Dispose();

            // Another query whit using DataReader for more of one returned column

            string queryInfo =
                "SELECT [EmployeeID],[FirstName],[LastName],[JobTitle] FROM [Employees] ORDER BY [EmployeeID]";
            using SqlConnection connectionInfo = new SqlConnection(Config.ConnectionString);
            connectionInfo.Open();
            SqlCommand commandInfo = new SqlCommand(queryInfo, connectionInfo);
            using SqlDataReader infoReader = commandInfo.ExecuteReader();

            // Read() returns false if don't has any rows
            //            and true if has one or more rows
            while (infoReader.Read())  
            {
                int id = (int)infoReader["EmployeeID"];
                string firstName = (string)infoReader["FirstName"];
                string lastName = (string)infoReader["LastName"];
                string jobTitle = (string)infoReader["JobTitle"];
                Console.WriteLine($"{id}. {firstName} {lastName} -> {jobTitle}");
            }

            infoReader.Close();
            connectionInfo.Close();
            sqlConnection.Dispose();
        }
    }
}
