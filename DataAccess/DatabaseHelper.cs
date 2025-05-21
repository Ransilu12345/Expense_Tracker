using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace Expense_Tracker.DataAccess
{
    public static class DatabaseHelper
    {
        private static string dbPath = "UserData.db";

        public static void InitializeDatabase()
        {
            if (!File.Exists(dbPath))
            {
                using var connection = new SqliteConnection($"Data Source={dbPath}");
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @"
                    CREATE TABLE Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT NOT NULL UNIQUE,
                        Password TEXT NOT NULL
                    );";
                tableCmd.ExecuteNonQuery();
            }
        }

        public static bool RegisterUser(string username, string password)
        {
            try
            {
                using var connection = new SqliteConnection($"Data Source={dbPath}");
                connection.Open();

                // Check if user already exists
                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = @"SELECT COUNT(*) FROM Users WHERE Username = $username";
                checkCmd.Parameters.AddWithValue("$username", username);

                var count = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (count > 0) return false;

                // Insert new user
                var insertCmd = connection.CreateCommand();
                insertCmd.CommandText = @"INSERT INTO Users (Username, Password) VALUES ($username, $password)";
                insertCmd.Parameters.AddWithValue("$username", username);
                insertCmd.Parameters.AddWithValue("$password", password);

                insertCmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering user: {ex.Message}");
                return false;
            }
        }

        public static bool ValidateUser(string username, string password)
        {
            try
            {
                using var connection = new SqliteConnection($"Data Source={dbPath}");
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"SELECT COUNT(*) FROM Users WHERE Username = $username AND Password = $password";
                command.Parameters.AddWithValue("$username", username);
                command.Parameters.AddWithValue("$password", password);

                var result = Convert.ToInt32(command.ExecuteScalar());
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error validating user: {ex.Message}");
                return false;
            }
        }
    }
}
