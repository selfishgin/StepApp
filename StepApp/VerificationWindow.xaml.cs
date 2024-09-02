using System.Windows;

namespace StepApp
{
    public partial class VerificationWindow : Window
    {
        private readonly string _email;
        private readonly string _verificationCode;

        public VerificationWindow(string email, string verificationCode)
        {
            InitializeComponent();
            _email = email;
            _verificationCode = verificationCode;
        }

        private void Verify_Click(object sender, RoutedEventArgs e)
        {
            string enteredCode = txtVerificationCode.Text;

            if (enteredCode == _verificationCode)
            {
                MessageBox.Show("Verification successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                MainPage mainPage = new MainPage(_email);
                mainPage.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid verification code. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
