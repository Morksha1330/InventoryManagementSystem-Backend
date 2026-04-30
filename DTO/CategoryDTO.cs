namespace InventoryMgtSystem.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ProductCount { get; set; }
        public int SubCategoryCount { get; set; }
    }
}