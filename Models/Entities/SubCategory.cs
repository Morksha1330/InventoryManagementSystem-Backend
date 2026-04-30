using System.Text.Json.Serialization;

namespace InventoryMgtSystem.Models.Entities
{
    public class SubCategory : BaseEntity
    {
        public int CategoryId { get; set; }
        public string SubCategoryCode { get; set; }
        public string SubCategoryName { get; set; }
        public string SubCategoryDescription { get; set; }
        public bool Active { get; set; }

        [JsonIgnore]
        public virtual Category Category { get; set; }

    }
}
