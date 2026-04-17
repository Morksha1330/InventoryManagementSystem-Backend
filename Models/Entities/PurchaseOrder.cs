namespace InventoryMgtSystem.Models.Entities
{
    public class PurchaseOrder :BaseEntity
    {
        public int SupplierId { get; set; }
        public int UserId { get; set; }
        public DateTime PurchaseOrderedDate { get; set; }
        public decimal TotalAmount { get; set; }
        public required string Status { get; set; }
        public bool Active { get; set; }
        public string? PaymentType { get; set; }

    }
}
