namespace InventoryMgtSystem.Models.Entities
{
    public class Stock : BaseEntity
    {
        public int ProductId { get; set; }
        public int? QuantityInStock { get; set; }
        public DateTime LastUpdatedDate { get; set; }

    }
}
