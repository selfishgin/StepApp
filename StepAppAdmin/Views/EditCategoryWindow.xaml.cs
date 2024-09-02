using System.Windows;

namespace StepAppAdmin.Views
{
    public partial class EditCategoryWindow : Window
    {
        private int categoryId;

        public EditCategoryWindow(int categoryId, string name, string description)
        {
            InitializeComponent();

            this.categoryId = categoryId;
            CategoryNameTextBox.Text = name;
            CategoryDescriptionTextBox.Text = description;
        }

        private void SaveCategory_Click(object sender, RoutedEventArgs e)
        {
            string name = CategoryNameTextBox.Text;
            string description = CategoryDescriptionTextBox.Text;

            DatabaseHelper.UpdateCategory(categoryId, name, description);
            MessageBox.Show("Kateqoriya uğurla redaktə edildi.", "Uğur", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
