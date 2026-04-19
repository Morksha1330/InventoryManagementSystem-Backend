namespace InventoryMgtSystem.DTO
{
    public class UpdateProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string SKU { get; set; }
        public decimal UnitPrice { get; set; }
        public bool Active { get; set; }
    }
}
