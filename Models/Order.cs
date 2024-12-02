namespace DarazApp.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }  // User placing the order
        public int NumOfItems { get; set; }  // Quantity of items in the order
        public string OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        public string Address { get; set; }


        // Navigation property to Product
        public Product Product { get; set; }

        // Navigation property to User (assuming User is another entity in the system)
        public User User { get; set; }
    }

}
