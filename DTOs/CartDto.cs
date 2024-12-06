namespace DarazApp.DTOs
{
    public class CartDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<CartItemDto> CartItems { get; set; }
    }
}
