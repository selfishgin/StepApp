namespace StepApp.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalPrice { get; set; }
        public List<OrderItem> OrderItems { get; set; } // Sifarişdəki məhsulların siyahısı
    }
}
