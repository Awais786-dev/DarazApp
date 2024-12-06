namespace DarazApp.DTOs
{
    public class CartInputDto
    {
        public string UserId { get; set; }

        public List<CartItemInputDto> Items { get; set; }
    }
}
