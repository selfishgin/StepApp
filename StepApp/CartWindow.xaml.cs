using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using StepApp.Models;

namespace StepApp
{
    public partial class CartWindow : Window
    {
        private Cart userCart;
        private string userEmail;

        public CartWindow(Cart cart, string email)
        {
            InitializeComponent();
            userCart = cart;
            userEmail = email;
            LoadCartItems();
        }

        private void LoadCartItems()
        {
            CartListView.ItemsSource = userCart.GetItems();
            TotalPriceTextBlock.Text = userCart.GetTotalPrice().ToString("C2");
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in userCart.GetItems())
            {
                UpdateProductStock(item.ProductId, item.Quantity);
            }

            // Qəbzi mətn olaraq yaradın
            string receiptText = CreateReceiptText();

            // Email vasitəsilə qəbzi göndərin
            SendEmailWithReceipt(receiptText);

            // Qəbzi göndərdikdən sonra səbəti təmizləyin
            userCart.ClearCart();
            LoadCartItems();

            MessageBox.Show("Checkout completed successfully! Receipt sent to your email.", "Checkout", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private string CreateReceiptText()
        {
            StringBuilder receipt = new StringBuilder();

            receipt.AppendLine("Ödəniş Qəbzi");
            receipt.AppendLine("Bizi seçdiyiniz üçün təşəkkür edirik!");
            receipt.AppendLine($"Tarix: {DateTime.Now.ToShortDateString()}");
            receipt.AppendLine($"Ümumi qiymət: {userCart.GetTotalPrice().ToString("C2")}");
            receipt.AppendLine();
            receipt.AppendLine("Məhsullar:");

            foreach (var item in userCart.GetItems())
            {
                receipt.AppendLine($"{item.ProductName} - {item.Quantity} x {item.Price:C2}");
            }

            return receipt.ToString();
        }


        private void SendEmailWithReceipt(string receiptText)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("ahmedhuseynli2000@gmail.com");
            mail.To.Add(userEmail);
            mail.Subject = "Sizin ödəniş qəbziniz";
            mail.Body = receiptText; 

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("ahmedhuseynli2000@gmail.com", "ekph mmjg jadi vior"),
                EnableSsl = true,
            };

            try
            {
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while sending the receipt email: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is CartItem cartItem)
            {
                userCart.RemoveFromCart(cartItem.ProductId);
                LoadCartItems();
            }
        }

        private void UpdateProductStock(int productId, int quantitySold)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=localhost;Initial Catalog=OnlineMarketDB;Integrated Security=True"))
            {
                conn.Open();
                string query = "UPDATE Products SET Stock = Stock - @Quantity WHERE Id = @ProductId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Quantity", quantitySold);
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    cmd.ExecuteNonQuery();
                }
            }
        }


    }
}
