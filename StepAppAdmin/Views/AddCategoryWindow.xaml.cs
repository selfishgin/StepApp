using System.Windows;

namespace StepAppAdmin.Views
{
    public partial class AddCategoryWindow : Window
    {
        public AddCategoryWindow()
        {
            InitializeComponent();
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            string name = CategoryNameTextBox.Text;
            string description = CategoryDescriptionTextBox.Text;

            DatabaseHelper.InsertCategory(name, description);
            MessageBox.Show("Kateqoriya uğurla əlavə edildi.", "Uğur", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }


    }
}
