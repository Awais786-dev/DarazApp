namespace DarazApp.Models
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }

        // Address Details
        public string Province { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string StreetDetails { get; set; }
        public string Address { get; set; }

        // Navigation properties
        public User User { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }



}
