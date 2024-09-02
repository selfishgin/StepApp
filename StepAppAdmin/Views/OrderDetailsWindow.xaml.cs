using System.Collections.Generic;
using System.Windows;

namespace StepAppAdmin.Views
{
    public partial class OrderDetailsWindow : Window
    {
        public OrderDetailsWindow(List<OrderItem> orderItems)
        {
            InitializeComponent();
            OrderItemsListView.ItemsSource = orderItems;
        }
    }
}
