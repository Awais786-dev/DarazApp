namespace DarazApp.Models
{
    public class Order : BaseEntity
    {
       
        public int ProductId { get; set; }
        public string UserId { get; set; }  // User placing the order
        public int NumOfItems { get; set; }  // Quantity of items in the order
        public string OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }


        public string Address { get; set; }


        // Navigation property to Product
        public Product Product { get; set; }

        // Navigation property to User (assuming User is another entity in the system)
        public User User { get; set; }
    }

}
