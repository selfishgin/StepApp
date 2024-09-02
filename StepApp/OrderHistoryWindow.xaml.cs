using System.Data;
using System.Windows;

namespace StepApp
{
    public partial class OrderHistoryWindow : Window
    {
        public OrderHistoryWindow(string userEmail)
        {
            InitializeComponent();
            LoadOrderHistory(userEmail);
        }

        private void LoadOrderHistory(string userEmail)
        {
            DataTable orderHistory = DatabaseHelper.GetOrderHistory(userEmail);
            OrderHistoryList.ItemsSource = orderHistory.DefaultView;
        }
    }
}
