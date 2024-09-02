using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace StepApp
{
    public partial class ProfileWindow : Window
    {
        public ProfileWindow()
        {
            InitializeComponent();
            LoadUserProfile();
        }

        private void LoadUserProfile()
        {
            // 
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text;
            string surname = txtSurname.Text;
            DateTime? dateOfBirth = dpDateOfBirth.SelectedDate;
            string email = txtEmail.Text;

            if (dateOfBirth.HasValue)
            {
                DatabaseHelper.UpdateUserProfile(email, name, surname, dateOfBirth.Value);
                MessageBox.Show("Profile information updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Please select a valid date of birth.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            if (pwdNewPassword.Password == pwdConfirmPassword.Password)
            {
                string newHashedPassword = HashPassword(pwdNewPassword.Password);
                DatabaseHelper.UpdateUserPassword(email, newHashedPassword);
                MessageBox.Show("Password changed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("New password and confirmation do not match.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewOrderHistory_Click(object sender, RoutedEventArgs e)
        {
            OrderHistoryWindow orderHistoryWindow = new OrderHistoryWindow(txtEmail.Text);
            orderHistoryWindow.ShowDialog();
        }

        private void AddCard_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("New card added.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void DeleteCard_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Card deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
