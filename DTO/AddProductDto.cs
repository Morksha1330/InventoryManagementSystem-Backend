namespace InventoryMgtSystem.DTO
{
    public class AddProductDto
    {
        public string SKU { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public int TotalQuantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
