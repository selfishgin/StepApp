using StepApp.Models;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace StepApp
{
    public partial class MainPage : Window
    {
        private Cart userCart = new Cart(); 
        private string userEmail; 

        public MainPage(string email)
        {
            InitializeComponent();
            userEmail = email;
            LoadCategories(); 
            LoadProducts();   
        }

        private void LoadCategories()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=OnlineMarketDB;Integrated Security=True"))
            {
                conn.Open();
                string query = "SELECT Name FROM Categories";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CategoryFilter.Items.Add(reader["Name"].ToString());
                        }
                    }
                }
            }
        }

        private void LoadProducts(string filter = null)
        {
            DataTable table = new DataTable();

            using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=OnlineMarketDB;Integrated Security=True"))
            {
                conn.Open();
                string query = "SELECT Id, Name, Price, Stock, ImagePath FROM Products";

                if (!string.IsNullOrEmpty(filter))
                {
                    query += " WHERE " + filter;
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(table);
                    }
                }
            }

            ProductGrid.Children.Clear();

            foreach (DataRow row in table.Rows)
            {
                var productPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness(10)
                };

                var productImage = new Image
                {
                    Source = new BitmapImage(new Uri(row["ImagePath"].ToString(), UriKind.RelativeOrAbsolute)),
                    Width = 100,
                    Height = 100,
                    Margin = new Thickness(5)
                };

                var productName = new TextBlock
                {
                    Text = row["Name"].ToString(),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(5)
                };

                var productPrice = new TextBlock
                {
                    Text = "Price: " + row["Price"].ToString() + " AZN",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(5)
                };

                var productStock = new TextBlock
                {
                    Text = "In Stock: " + row["Stock"].ToString(),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(5)
                };

                var addToCartButton = new Button
                {
                    Content = "Add to Cart",
                    Margin = new Thickness(5),
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                addToCartButton.Click += (sender, e) => AddToCart_Click(sender, e, int.Parse(row["Id"].ToString()));

                productPanel.Children.Add(productImage);
                productPanel.Children.Add(productName);
                productPanel.Children.Add(productPrice);
                productPanel.Children.Add(productStock);
                productPanel.Children.Add(addToCartButton);

                ProductGrid.Children.Add(productPanel);

                productPanel.MouseDown += (sender, e) => ProductPanel_MouseDown(sender, e, int.Parse(row["Id"].ToString()));
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e, int productId)
        {
            string productName = string.Empty;
            decimal price = 0;
            string imagePath = string.Empty;

            using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=OnlineMarketDB;Integrated Security=True"))
            {
                conn.Open();
                string query = "SELECT Name, Price, ImagePath FROM Products WHERE Id = @ProductId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            productName = reader["Name"].ToString();
                            price = Convert.ToDecimal(reader["Price"]);
                            imagePath = reader["ImagePath"].ToString();
                        }
                    }
                }
            }

            userCart.AddToCart(productId, productName, price, imagePath);

            MessageBox.Show($"Product {productName} added to cart.");
        }

        private void ProductPanel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e, int productId)
        {
            var productDetailWindow = new ProductDetailWindow(productId, userCart);
            productDetailWindow.ShowDialog();
        }

        private void Cart_Click(object sender, RoutedEventArgs e)
        {
            var cartWindow = new CartWindow(userCart, userEmail);
            cartWindow.ShowDialog();
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            string categoryFilter = CategoryFilter.SelectedItem?.ToString();
            string minPrice = MinPrice.Text;
            string maxPrice = MaxPrice.Text;

            string filter = "";

            if (!string.IsNullOrEmpty(categoryFilter))
            {
                filter += $"CategoryId = {GetCategoryIdByName(categoryFilter)}";
            }

            if (!string.IsNullOrEmpty(minPrice))
            {
                if (!string.IsNullOrEmpty(filter)) filter += " AND ";
                filter += $"Price >= {minPrice}";
            }

            if (!string.IsNullOrEmpty(maxPrice))
            {
                if (!string.IsNullOrEmpty(filter)) filter += " AND ";
                filter += $"Price <= {maxPrice}";
            }

            LoadProducts(filter);
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            ProfileWindow profileWindow = new ProfileWindow();
            profileWindow.ShowDialog();
        }

        private void CategoryFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryFilter.SelectedItem != null)
            {
                string selectedCategoryName = CategoryFilter.SelectedItem.ToString();

                int selectedCategoryId = GetCategoryIdByName(selectedCategoryName);

                string filter = $"CategoryId = {selectedCategoryId}";
                LoadProducts(filter);
            }
        }

        private int GetCategoryIdByName(string categoryName)
        {
            int categoryId = 0;

            using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=OnlineMarketDB;Integrated Security=True"))
            {
                conn.Open();
                string query = "SELECT Id FROM Categories WHERE Name = @CategoryName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CategoryName", categoryName);

                    categoryId = (int)cmd.ExecuteScalar();
                }
            }

            return categoryId;
        }
    }
}
