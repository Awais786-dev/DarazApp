namespace DarazApp.DTOs
{
    public class CheckoutCartDto
    {
        public string UserId { get; set; }

        // Address Fields
        public string Province { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string StreetDetails { get; set; }
        public string Address { get; set; }
    }

}
