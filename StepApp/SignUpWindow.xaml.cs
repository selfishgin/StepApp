using System;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace StepApp
{
    public partial class SignUpWindow : Window
    {
        public SignUpWindow()
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

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text;
            string surname = txtSurname.Text;
            DateTime dateOfBirth = dpDateOfBirth.SelectedDate.Value;
            string email = txtEmail.Text;
            string password = pwdPassword.Password;

            string hashedPassword = HashPassword(password);

            DatabaseHelper.InsertUser(name, surname, dateOfBirth, email, hashedPassword);


            
            string verificationCode = GenerateVerificationCode();
            SendVerificationEmail(email, verificationCode);

            VerificationWindow verificationWindow = new VerificationWindow(email, verificationCode);
            verificationWindow.Show();
            this.Close();
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

        private string GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private void SendVerificationEmail(string email, string verificationCode)
        {
            MailMessage message = new MailMessage("ahmedhuseynli2000@gmail.com", email);
            message.Subject = "Verification Code";
            message.Body = $"Your verification code is {verificationCode}";

            SmtpClient client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("ahmedhuseynli2000@gmail.com", "ekph mmjg jadi vior"),
                EnableSsl = true
            };

            client.Send(message);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            
            MainWindow mainWindow = new MainWindow(); 
            mainWindow.Show();
            this.Close(); 
        }
    }
}
