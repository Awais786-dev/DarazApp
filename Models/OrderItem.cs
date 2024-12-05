namespace DarazApp.Models
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; set; }  // Foreign key to Order
        public int ProductId { get; set; }  // Foreign key to Product
        public int Quantity { get; set; }  // Quantity of the product
        public decimal Price { get; set; }  // Price of the product at the time of purchase

        // Navigation properties
        public Order Order { get; set; }  // Parent Order
        public Product Product { get; set; }  // Associated Product
    }

}
