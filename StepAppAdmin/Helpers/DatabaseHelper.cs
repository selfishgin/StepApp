using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using StepAppAdmin.Models;
using StepAppAdmin.Views;

namespace StepAppAdmin
{
    public static class DatabaseHelper
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

        public static List<User> GetUsers()
        {
            var users = new List<User>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Id, Name, Surname, DateOfBirth, Email FROM Users", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Surname = reader.GetString(2),
                                DateOfBirth = reader.GetDateTime(3),
                                Email = reader.GetString(4)
                            });
                        }
                    }
                }
            }

            return users;
        }

        public static void DeleteUser(int userId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Users WHERE Id = @UserId", conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static List<Product> GetProducts()
        {
            var products = new List<Product>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Id, Name, Description, Price, Stock, ImagePath, CategoryId FROM Products", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2),
                                Price = reader.GetDecimal(3),
                                Stock = reader.GetInt32(4),
                                ImagePath = reader.GetString(5),
                                CategoryId = reader.GetInt32(6)
                            });
                        }
                    }
                }
            }

            return products;
        }

        public static void DeleteProduct(int productId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Products WHERE Id = @ProductId", conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void InsertProduct(string name, string description, decimal price, int stock, int categoryId, string imagePath)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Products (Name, Description, Price, Stock, ImagePath, CategoryId) VALUES (@Name, @Description, @Price, @Stock, @ImagePath, @CategoryId)", conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Stock", stock);
                    cmd.Parameters.AddWithValue("@ImagePath", imagePath);
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateProduct(int productId, string name, string description, decimal price, int stock, string imagePath, int categoryId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE Products SET Name = @Name, Description = @Description, Price = @Price, Stock = @Stock, ImagePath = @ImagePath, CategoryId = @CategoryId WHERE Id = @ProductId", conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Stock", stock);
                    cmd.Parameters.AddWithValue("@ImagePath", imagePath);
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static List<Order> GetOrders()
        {
            var orders = new List<Order>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Id, UserId, Address, Quantity, TotalPrice, OrderDate FROM Orders", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                Id = reader.GetInt32(0),
                                UserId = reader.GetInt32(1),
                                Address = reader.GetString(2),
                                Quantity = reader.GetInt32(3),
                                TotalPrice = reader.GetDecimal(4),
                                OrderDate = reader.GetDateTime(5)
                            });
                        }
                    }
                }
            }

            return orders;
        }

        public static void DeleteOrder(int orderId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Orders WHERE Id = @OrderId", conn))
                {
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void InsertOrder(int userId, string address, int quantity, decimal totalPrice)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Orders (UserId, Address, Quantity, TotalPrice) VALUES (@UserId, @Address, @Quantity, @TotalPrice)", conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@TotalPrice", totalPrice);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static List<OrderItem> GetOrderItems(int orderId)
        {
            var orderItems = new List<OrderItem>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT ProductId, Quantity, Price FROM OrderItems WHERE OrderId = @OrderId", conn))
                {
                    cmd.Parameters.AddWithValue("@OrderId", orderId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orderItems.Add(new OrderItem
                            {
                                ProductId = reader.GetInt32(0),
                                Quantity = reader.GetInt32(1),
                                Price = reader.GetDecimal(2)
                            });
                        }
                    }
                }
            }

            return orderItems;
        }


        public static List<Category> GetCategories()
        {
            var categories = new List<Category>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Id, Name, Description FROM Categories", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(new Category
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2)
                            });
                        }
                    }
                }
            }

            return categories;
        }

        public static void InsertCategory(string name, string description)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Categories (Name, Description) VALUES (@Name, @Description)", conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Description", description);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateCategory(int categoryId, string name, string description)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE Categories SET Name = @Name, Description = @Description WHERE Id = @CategoryId", conn))
                {
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Description", description);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteCategory(int categoryId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Categories WHERE Id = @CategoryId", conn))
                {
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                    cmd.ExecuteNonQuery();
                }
            }
        }



    }



    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImagePath { get; set; }
        public int CategoryId { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Address { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }

    public class OrderItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice => Quantity * Price; 
    }
}
