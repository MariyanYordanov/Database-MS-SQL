namespace _01.ADO_NET_Exercise
{
    using System;
    using System.Security;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.ComponentModel;

    public static class Config
    {
      
        public static string ConnectionString = @"Server=EXECUTOR;Database=SoftUni;Integrated Security=True;";
            
        public static SecureString Password()
        {
            // Just Mikrosoft docs example for Main()
            // Instantiate the secure string.
            SecureString securePwd = new SecureString();
            ConsoleKeyInfo key;

            Console.Write("Enter password: ");
            do
            {
                key = Console.ReadKey(true);

                // Ignore any key out of range.
                if (((int)key.Key) >= 65 && ((int)key.Key <= 90))
                {
                    // Append the character to the password.
                    securePwd.AppendChar(key.KeyChar);
                    Console.Write("*");
                }
                // Exit if Enter key is pressed.
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();

            try
            {
                Process.Start("Notepad.exe", "MyUser", securePwd, "MYDOMAIN");
            }
            catch (Win32Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                securePwd.Dispose();
            }
            

            return securePwd;
        }

        public static SqlCredential Authenticator(string userID, SecureString password)
        {
            SqlCredential sqlCredential = new SqlCredential(userID, password);

            return sqlCredential;
        }
    }
}
