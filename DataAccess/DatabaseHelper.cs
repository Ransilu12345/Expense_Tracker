using Expense_Tracker.Models;
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
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = @"
        CREATE TABLE IF NOT EXISTS Users (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Username TEXT NOT NULL UNIQUE,
            Password TEXT NOT NULL
        );

        CREATE TABLE IF NOT EXISTS Inventory (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            ItemName TEXT NOT NULL,
            Quantity INTEGER NOT NULL,
            Price REAL NOT NULL
        );

        CREATE TABLE IF NOT EXISTS Sales (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            ItemName TEXT NOT NULL,
            Quantity INTEGER NOT NULL,
            Price REAL NOT NULL,
            Date TEXT NOT NULL
        );";

            tableCmd.ExecuteNonQuery();
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


        public static void AddInventoryItem(string name, int quantity, double price)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Inventory (ItemName, Quantity, Price) 
                        VALUES ($name, $quantity, $price);";
            cmd.Parameters.AddWithValue("$name", name);
            cmd.Parameters.AddWithValue("$quantity", quantity);
            cmd.Parameters.AddWithValue("$price", price);

            cmd.ExecuteNonQuery();
        }

        public static List<InventoryItem> GetAllInventoryItems()
        {
            var items = new List<InventoryItem>();

            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, ItemName, Quantity, Price FROM Inventory";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                items.Add(new InventoryItem
                {
                    Id = reader.GetInt32(0),
                    ItemName = reader.GetString(1),
                    Quantity = reader.GetInt32(2),
                    Price = reader.GetDouble(3)
                });
            }

            return items;
        }



        public static void AddSaleItem(string name, int quantity, double price)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Sales (ItemName, Quantity, Price, Date) 
                        VALUES ($name, $quantity, $price, $date);";
            cmd.Parameters.AddWithValue("$name", name);
            cmd.Parameters.AddWithValue("$quantity", quantity);
            cmd.Parameters.AddWithValue("$price", price);
            cmd.Parameters.AddWithValue("$date", DateTime.Now.ToString("yyyy-MM-dd"));

            cmd.ExecuteNonQuery();
        }

        public static List<SaleItem> GetAllSales()
        {
            var sales = new List<SaleItem>();

            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, ItemName, Quantity, Price, Date FROM Sales";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                sales.Add(new SaleItem
                {
                    Id = reader.GetInt32(0),
                    ProductName = reader.GetString(1),
                    Quantity = reader.GetInt32(2),
                    Price = reader.GetDouble(3),
                    Date = DateTime.Parse(reader.GetString(4))
                });

            }

            return sales;
        }


        public static double GetTotalSales()
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT SUM(Quantity * Price) FROM Sales";

            var result = cmd.ExecuteScalar();
            return result == DBNull.Value ? 0 : Convert.ToDouble(result);
        }

        public static double GetTotalExpenses()
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT SUM(Quantity * Price) FROM Inventory";

            var result = cmd.ExecuteScalar();
            return result == DBNull.Value ? 0 : Convert.ToDouble(result);
        }


        public static void UpdateInventoryItem(int id, string name, int quantity, double price)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"UPDATE Inventory SET ItemName = $name, Quantity = $quantity, Price = $price WHERE Id = $id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.Parameters.AddWithValue("$name", name);
            cmd.Parameters.AddWithValue("$quantity", quantity);
            cmd.Parameters.AddWithValue("$price", price);
            cmd.ExecuteNonQuery();
        }

        public static void DeleteInventoryItem(int id)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM Inventory WHERE Id = $id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }


        public static void UpdateSalesItem(int id, string name, int quantity, double price)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"UPDATE Sales SET ItemName = $name, Quantity = $quantity, Price = $price WHERE Id = $id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.Parameters.AddWithValue("$name", name);
            cmd.Parameters.AddWithValue("$quantity", quantity);
            cmd.Parameters.AddWithValue("$price", price);
            cmd.ExecuteNonQuery();
        }

        public static void DeleteSalesItem(int id)
        {
            using var connection = new SqliteConnection($"Data Source={dbPath}");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM Sales WHERE Id = $id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }

    }
}
