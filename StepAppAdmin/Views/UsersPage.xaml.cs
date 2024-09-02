using System.Windows;
using System.Windows.Controls;

namespace StepAppAdmin.Views
{
    public partial class UsersPage : Page
    {
        public UsersPage()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            UsersDataGrid.ItemsSource = DatabaseHelper.GetUsers();
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is User selectedUser)
            {
                DatabaseHelper.DeleteUser(selectedUser.Id);
                LoadUsers(); 
            }
            else
            {
                MessageBox.Show("Silmək üçün bir istifadəçi seçin.", "Xəta", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
