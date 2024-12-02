namespace DarazApp.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public int NumOfItems { get; set; }
        public string? OrderStatus { get; set; }
        public string PaymentMethod { get; set; }
        public decimal? TotalAmount { get; set; }
        public string Address { get; set; }



    }
}
