namespace InventoryMgtSystem.Models.Entities
{
    public class Permission : BaseEntity
    {
        public int FormId { get; set; }
        public int RoleId { get; set; }
        public bool Active { get; set; }
    }
}
