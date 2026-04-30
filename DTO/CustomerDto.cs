namespace InventoryMgtSystem.DTO
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public bool Active { get; set; }
        public int? CreatedUser { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}