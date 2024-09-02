using System.Windows;
using Microsoft.Win32;
using StepAppAdmin.Models;

namespace StepAppAdmin.Views
{
    public partial class AddProductWindow : Window
    {
        public AddProductWindow()
        {
            InitializeComponent();
            LoadCategories();
        }

        private void LoadCategories()
        {
            CategoryComboBox.ItemsSource = DatabaseHelper.GetCategories();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            string name = ProductNameTextBox.Text;
            string description = ProductDescriptionTextBox.Text;
            if (decimal.TryParse(ProductPriceTextBox.Text, out decimal price) && int.TryParse(ProductStockTextBox.Text, out int stock))
            {
                string imagePath = ProductImagePathTextBox.Text;
                int categoryId = (int)CategoryComboBox.SelectedValue; // Category ID

                DatabaseHelper.InsertProduct(name, description, price, stock, categoryId, imagePath);
                MessageBox.Show("Məhsul uğurla əlavə edildi.", "Uğur", MessageBoxButton.OK, MessageBoxImage.Information);
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
