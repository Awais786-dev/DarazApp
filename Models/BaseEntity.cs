namespace DarazApp.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public bool IsActive { get; set; } = true; // Default value
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Default to current UTC time
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow; // Default to current UTC time
    }

}
