namespace InventoryMgtSystem.Models.Entities
{
    public class SalesOrder :BaseEntity
    {
        public DateTime OrderDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? PaymentType { get; set; }
        public string? Status { get; set; }
        public int CustomerId { get; set; }
    }
}
