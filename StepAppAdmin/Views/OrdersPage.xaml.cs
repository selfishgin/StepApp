using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace StepAppAdmin.Views
{
    public partial class OrdersPage : Page
    {
        public OrdersPage()
        {
            InitializeComponent();
            LoadOrders();
        }

        private void LoadOrders()
        {
            OrdersDataGrid.ItemsSource = DatabaseHelper.GetOrders();
        }

        private void ViewOrderDetails_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is Order selectedOrder)
            {
                List<OrderItem> orderItems = DatabaseHelper.GetOrderItems(selectedOrder.Id);
                var orderDetailsWindow = new OrderDetailsWindow(orderItems);
                orderDetailsWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Detalları görmək üçün bir sifariş seçin.", "Xəta", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is Order selectedOrder)
            {
                DatabaseHelper.DeleteOrder(selectedOrder.Id);
                LoadOrders(); 
            }
            else
            {
                MessageBox.Show("Silmək üçün bir sifariş seçin.", "Xəta", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
