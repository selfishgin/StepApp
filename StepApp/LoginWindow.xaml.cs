using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace StepApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }



        private void chkShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            pwdPasswordVisible.Text = pwdPassword.Password;
            pwdPasswordVisible.Visibility = Visibility.Visible;
            pwdPassword.Visibility = Visibility.Collapsed;
        }

        private void chkShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            pwdPassword.Password = pwdPasswordVisible.Text;
            pwdPasswordVisible.Visibility = Visibility.Collapsed;
            pwdPassword.Visibility = Visibility.Visible;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string password = pwdPassword.Password; 
            string hashedPassword = HashPassword(password);

            
            string storedHashedPassword = DatabaseHelper.GetUserPassword(email);

            if (storedHashedPassword == hashedPassword)
            {
                
                MainPage mainPage = new MainPage(email);
                mainPage.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid email or password.");
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(); 
            mainWindow.Show();
            this.Close(); 
        }


    }
}
