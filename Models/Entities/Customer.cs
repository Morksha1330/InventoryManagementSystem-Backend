namespace InventoryMgtSystem.Models.Entities
{
    public class Customer : BaseEntity
    {
        public required string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
