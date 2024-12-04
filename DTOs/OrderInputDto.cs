namespace DarazApp.DTOs
{
    public class OrderInputDto
    {
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public int NumOfItems { get; set; }
        public string PaymentMethod { get; set; }
        public string Address { get; set; }
    }
}
