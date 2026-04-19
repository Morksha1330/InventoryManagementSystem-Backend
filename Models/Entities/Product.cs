
using System.Text.Json.Serialization;

namespace InventoryMgtSystem.Models.Entities
{
    public class Product : BaseEntity
    {
        public string SKU { get; set; }
        public required string ProductName { get; set; }
        public required int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public int TotalQuantity { get; set; }
        public required decimal UnitPrice { get; set; }
        public bool Active { get; set; }
        public int ReOrderLevel { get; set; }

        // Navigation property
        [JsonIgnore]
        public virtual Category Category { get; set; }

    }
}
