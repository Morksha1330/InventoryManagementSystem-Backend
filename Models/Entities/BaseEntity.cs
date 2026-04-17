namespace InventoryMgtSystem.Models.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public int? CreatedUser { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

    }
}
