using System.Windows;
using System.Windows.Controls;

namespace StepAppAdmin.Views
{
    public partial class ProductsPage : Page
    {
        public ProductsPage()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            ProductsDataGrid.ItemsSource = DatabaseHelper.GetProducts();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            var addProductWindow = new AddProductWindow();
            addProductWindow.ShowDialog();
            LoadProducts(); 
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem is Product selectedProduct)
            {
                var editProductWindow = new EditProductWindow(selectedProduct.Id, selectedProduct.Name, selectedProduct.Description, selectedProduct.Price, selectedProduct.Stock, selectedProduct.ImagePath, selectedProduct.CategoryId);
                editProductWindow.ShowDialog();
                LoadProducts(); 
            }
            else
            {
                MessageBox.Show("Redaktə etmək üçün bir məhsul seçin.", "Xəta", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedItem is Product selectedProduct)
            {
                DatabaseHelper.DeleteProduct(selectedProduct.Id);
                LoadProducts(); 
            }
            else
            {
                MessageBox.Show("Silmək üçün bir məhsul seçin.", "Xəta", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
