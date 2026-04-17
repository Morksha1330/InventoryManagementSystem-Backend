namespace InventoryMgtSystem.Models.Entities
{
    public class StockTransaction : BaseEntity
    {
        public int ProductId { get; set; }
        public int QuantityAvailable { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
