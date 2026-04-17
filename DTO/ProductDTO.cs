namespace InventoryMgtSystem.DTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public required string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string SKU { get; set; }
        public decimal UnitPrice { get; set; }
        public bool Status { get; set; }

    }

    public class CustomProduct
    {
        public string ProName { get; set; }
        public string ProductCode { get; set; }
    }
}
