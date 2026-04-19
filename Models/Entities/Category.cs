using System.Text.Json.Serialization;

namespace InventoryMgtSystem.Models.Entities
{
    public class Category : BaseEntity
    {
        public required string CategoryCode { get; set; }
        public required string CategoryName { get; set; }
        public required string Description { get; set; }
        public bool Active { get; set; }

        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<SubCategory> SubCategories { get; set; }

    }
}
