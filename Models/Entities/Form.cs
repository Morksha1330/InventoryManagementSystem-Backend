namespace InventoryMgtSystem.Models.Entities
{
    public class Form:BaseEntity
    {
        public required string FormName { get; set; }
        public required int Role { get; set; }
        public bool Active { get; set; }

    }
}
