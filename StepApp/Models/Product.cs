namespace StepApp.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public string ImagePath { get; set; } // Məhsul şəkli üçün yol
    }
}
