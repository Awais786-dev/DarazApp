namespace DarazApp.DTOs
{
    public class OrderInputDto
    {
        public string UserId { get; set; }
        public List<OrderItemInputDto> Items { get; set; }
        public string PaymentMethod { get; set; }

        // Address Details
        public string Province { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string StreetDetails { get; set; }
        public string Address { get; set; }
    }

}
