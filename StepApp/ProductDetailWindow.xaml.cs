//"Data Source = localhost; Initial Catalog = OnlineMarketDB; Integrated Security = True"
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Media.Imaging;
using StepApp.Models;

namespace StepApp
{
    public partial class ProductDetailWindow : Window
    {
        private Cart userCart;
        private int productId; 
        private decimal productPrice; 
        private string imagePath; 

        public ProductDetailWindow(int productId, Cart cart)
        {
            InitializeComponent();
            this.productId = productId; 
            this.userCart = cart;
            LoadProductDetails(); 
        }

        private void LoadProductDetails()
        {
            string connectionString = "Data Source = localhost; Initial Catalog = OnlineMarketDB; Integrated Security = True"; // Verilənlər bazası bağlantı sətiri

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Name, Price, ImagePath FROM Products WHERE Id = @ProductId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", productId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string productName = reader["Name"].ToString();
                            productPrice = Convert.ToDecimal(reader["Price"]);
                            imagePath = reader["ImagePath"].ToString();

                            ProductNameTextBlock.Text = productName;
                            ProductPriceTextBlock.Text = $"Price: {productPrice} AZN";

                            if (!string.IsNullOrEmpty(imagePath))
                            {
                                ProductImage.Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
                            }
                            else
                            {
                                ProductImage.Source = new BitmapImage(new Uri("defaultImagePathHere", UriKind.RelativeOrAbsolute));
                            }
                        }
                        else
                        {
                            MessageBox.Show("Product not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            this.Close();
                        }
                    }
                }
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            userCart.AddToCart(productId, ProductNameTextBlock.Text, productPrice, imagePath);
            MessageBox.Show($"Product {ProductNameTextBlock.Text} added to cart.");
        }
    }
}

