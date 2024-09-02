using System.Collections.Generic;
using System.Linq;

namespace StepApp.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImagePath { get; set; }  
    }

    public class Cart
    {
        private List<CartItem> items = new List<CartItem>();

        public void AddToCart(int productId, string productName, decimal price, string imagePath, int quantity = 1)
        {
            var existingItem = items.FirstOrDefault(item => item.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                items.Add(new CartItem
                {
                    ProductId = productId,
                    ProductName = productName,
                    Price = price,
                    Quantity = quantity,
                    ImagePath = imagePath
                });
            }
        }

        public List<CartItem> GetItems()
        {
            return items;
        }

        public decimal GetTotalPrice()
        {
            return items.Sum(item => item.Price * item.Quantity);
        }

        public void RemoveFromCart(int productId)
        {
            var item = items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                items.Remove(item);
            }
        }

        public void ClearCart()
        {
            items.Clear();
        }
    }
}
