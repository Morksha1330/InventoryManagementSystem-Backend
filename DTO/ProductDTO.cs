namespace InventoryMgtSystem.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string SKU { get; set; }
        public string ProductName { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public int TotalQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public bool Active { get; set; }

        public DateTime? CreatedDate { get; set; }
        public int? SubCategoryId { get; set; }
        public string? SubCategoryName { get; set; }
        public int? ReOrderLevel { get; set; }

    }

    public class CustomProduct
    {
        public string ProName { get; set; }
        public string ProductCode { get; set; }
    }
}
