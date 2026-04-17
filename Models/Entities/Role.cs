namespace InventoryMgtSystem.Models.Entities
{
    public class Role :BaseEntity
    {
        public required string RoleName { get; set; }
        public bool Active { get; set; }

    }
}
