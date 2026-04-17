namespace InventoryMgtSystem.Models.Entities
{
    public class Supplier : BaseEntity
    {
        public required string Name { get; set; }
        public required string Contact { get; set; }
        public required string Address { get; set; }
        public required string Email { get; set; }
        public bool Active { get; set; }

    }
}
