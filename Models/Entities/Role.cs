using System.Text.Json.Serialization;

namespace InventoryMgtSystem.Models.Entities
{
    public class Role :BaseEntity
    {
        public required string RoleName { get; set; }
        public bool Active { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }
    }
}
