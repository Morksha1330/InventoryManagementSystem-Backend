namespace InventoryMgtSystem.Models.Entities
{
    public class Product : BaseEntity
    {
        public string SKU { get; set; }
        public required string ProductName { get; set; }
        public required int CategoryId { get; set; }
        public int TotalQuantity { get; set; }
        public required decimal UnitPrice { get; set; }
        public bool Active { get; set; }

    }
}
