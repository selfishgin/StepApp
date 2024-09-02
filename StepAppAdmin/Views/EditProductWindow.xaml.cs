using System.Windows;
using Microsoft.Win32;
using StepAppAdmin.Models;

namespace StepAppAdmin.Views
{
    public partial class EditProductWindow : Window
    {
        private int productId;

        public EditProductWindow(int productId, string name, string description, decimal price, int stock, string imagePath, int categoryId)
        {
            InitializeComponent();
            this.productId = productId;
            LoadCategories();
            ProductNameTextBox.Text = name;
            ProductDescriptionTextBox.Text = description;
            ProductPriceTextBox.Text = price.ToString();
            ProductStockTextBox.Text = stock.ToString();
            ProductImagePathTextBox.Text = imagePath;
            CategoryComboBox.SelectedValue = categoryId;
        }

        private void LoadCategories()
        {
            CategoryComboBox.ItemsSource = DatabaseHelper.GetCategories();
        }

        private void SaveProduct_Click(object sender, RoutedEventArgs e)
        {
            string name = ProductNameTextBox.Text;
            string description = ProductDescriptionTextBox.Text;
            if (decimal.TryParse(ProductPriceTextBox.Text, out decimal price) && int.TryParse(ProductStockTextBox.Text, out int stock))
            {
                string imagePath = ProductImagePathTextBox.Text;
                int categoryId = (int)CategoryComboBox.SelectedValue; // Category ID

                DatabaseHelper.UpdateProduct(productId, name, description, price, stock, imagePath, categoryId);
                MessageBox.Show("Məhsul uğurla redaktə edildi.", "Uğur", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Zəhmət olmasa düzgün məlumat daxil edin.", "Xəta", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BrowseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

            if (openFileDialog.ShowDialog() == true)
            {
                ProductImagePathTextBox.Text = openFileDialog.FileName;
            }
        }
    }
}
