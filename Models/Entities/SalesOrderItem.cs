namespace InventoryMgtSystem.Models.Entities
{
    public class SalesOrderItem : BaseEntity
    {
        public int? SaleOrderId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? SubTotal { get; set; }

    }
}
