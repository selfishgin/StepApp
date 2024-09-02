namespace StepAppAdmin.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Address { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }

        public string UserName { get; set; }
        public string UserEmail { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
