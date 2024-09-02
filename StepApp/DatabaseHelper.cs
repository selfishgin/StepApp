using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Newtonsoft.Json;
using StepApp.Models; 


namespace StepApp
{
    public static partial class DatabaseHelper
    {
        private static readonly string connectionString = "Data Source=localhost;Initial Catalog=OnlineMarketDB;Integrated Security=True";

        public static string GetUserPassword(string email)
        {
            string hashedPassword = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT PasswordHash FROM Users WHERE Email = @Email", conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            hashedPassword = reader["PasswordHash"].ToString();
                        }
                    }
                }
            }

            return hashedPassword;
        }

        public static void InsertUser(string name, string surname, DateTime dateOfBirth, string email, string hashedPassword)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Users (Name, Surname, DateOfBirth, Email, PasswordHash) VALUES (@Name, @Surname, @DateOfBirth, @Email, @PasswordHash)", conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Surname", surname);
                    cmd.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateUserProfile(string email, string name, string surname, DateTime dateOfBirth)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE Users SET Name = @Name, Surname = @Surname, DateOfBirth = @DateOfBirth WHERE Email = @Email", conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Surname", surname);
                    cmd.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                    cmd.Parameters.AddWithValue("@Email", email);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateUserPassword(string email, string newHashedPassword)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE Users SET PasswordHash = @PasswordHash WHERE Email = @Email", conn))
                {
                    cmd.Parameters.AddWithValue("@PasswordHash", newHashedPassword);
                    cmd.Parameters.AddWithValue("@Email", email);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static DataTable GetOrderHistory(string email)
        {
            DataTable orderHistory = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT o.Id AS OrderId, o.Date AS OrderDate, o.TotalPrice, oi.ProductId, oi.Stock " +
                                                        "FROM Orders o " +
                                                        "INNER JOIN OrderItems oi ON o.Id = oi.OrderId " +
                                                        "WHERE o.UserId = (SELECT Id FROM Users WHERE Email = @Email)", conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(orderHistory);
                    }
                }
            }

            return orderHistory;
        }

        public static DataTable GetProductsByCategory(int categoryId)
        {
            DataTable products = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Products WHERE CategoryId = @CategoryId", conn))
                {
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(products);
                    }
                }
            }

            return products;
        }

        public static void SaveCart(int userId, List<CartItem> items)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand deleteCmd = new SqlCommand("DELETE FROM Cart WHERE UserId = @UserId", conn))
                {
                    deleteCmd.Parameters.AddWithValue("@UserId", userId);
                    deleteCmd.ExecuteNonQuery();
                }

                foreach (var item in items)
                {
                    using (SqlCommand insertCmd = new SqlCommand("INSERT INTO Cart (UserId, ProductId, Stock) VALUES (@UserId, @ProductId, @Stock)", conn))
                    {
                        insertCmd.Parameters.AddWithValue("@UserId", userId);
                        insertCmd.Parameters.AddWithValue("@ProductId", item.ProductId);
                        insertCmd.Parameters.AddWithValue("@Stock", item.Quantity);

                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static List<CartItem> GetCartItems(int userId)
        {
            List<CartItem> items = new List<CartItem>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT ProductId, Stock FROM Cart WHERE UserId = @UserId", conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new CartItem
                            {
                                ProductId = reader.GetInt32(0),
                                Quantity = reader.GetInt32(1)
                            });
                        }
                    }
                }
            }

            return items;
        }

        public static void InsertOrder(int userId, string address, int Stock, List<OrderItem> orderItems)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO Orders (Address, UserId, Stock) OUTPUT INSERTED.ID VALUES (@Address, @UserId, @Stock)", conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Address", address);
                            cmd.Parameters.AddWithValue("@UserId", userId);
                            cmd.Parameters.AddWithValue("@Stock", Stock);

                            int orderId = (int)cmd.ExecuteScalar();

                            foreach (var item in orderItems)
                            {
                                using (SqlCommand itemCmd = new SqlCommand("INSERT INTO OrderItems (OrderId, ProductId, Stock) VALUES (@OrderId, @ProductId, @Stock)", conn, transaction))
                                {
                                    itemCmd.Parameters.AddWithValue("@OrderId", orderId);
                                    itemCmd.Parameters.AddWithValue("@ProductId", item.ProductId);
                                    itemCmd.Parameters.AddWithValue("@Stock", item.Stock);

                                    itemCmd.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }



    public class OrderItem
    {
        public int ProductId { get; set; }
        public int Stock { get; set; }
    }
}
