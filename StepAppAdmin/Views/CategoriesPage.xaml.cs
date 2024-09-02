using System.Windows;
using System.Windows.Controls;
using StepAppAdmin.Models;

namespace StepAppAdmin.Views
{
    public partial class CategoriesPage : Page
    {
        public CategoriesPage()
        {
            InitializeComponent();
            LoadCategories();
        }

        private void LoadCategories()
        {
            CategoriesDataGrid.ItemsSource = DatabaseHelper.GetCategories();
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            var addCategoryWindow = new AddCategoryWindow();
            addCategoryWindow.ShowDialog();
            LoadCategories(); 
        }

        private void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesDataGrid.SelectedItem is Category selectedCategory)
            {
                var editCategoryWindow = new EditCategoryWindow(selectedCategory.Id, selectedCategory.Name, selectedCategory.Description);
                editCategoryWindow.ShowDialog();
                LoadCategories(); 
            }
            else
            {
                MessageBox.Show("Redaktə etmək üçün bir kateqoriya seçin.", "Xəta", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesDataGrid.SelectedItem is Category selectedCategory)
            {
                DatabaseHelper.DeleteCategory(selectedCategory.Id);
                LoadCategories(); 
            }
            else
            {
                MessageBox.Show("Silmək üçün bir kateqoriya seçin.", "Xəta", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
