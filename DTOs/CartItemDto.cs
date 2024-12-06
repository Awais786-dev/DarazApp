namespace DarazApp.DTOs
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } // Optional, fetched from the Product table
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
