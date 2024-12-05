namespace DarazApp.DTOs
{
    public class OrderItemInputDto
    {
        public int ProductId { get; set; } // The ID of the product being ordered
        public int NumOfItems { get; set; } // The quantity of the product being ordered
    }
}
