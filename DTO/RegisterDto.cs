namespace InventoryMgtSystem.DTO
{
    public class RegisterDto
    {
        public int? Id { get; set; }
        public required string Name { get; set; }
        public required string Username { get; set; }
        public string? Phone { get; set; }
        public required string Email { get; set; }
        public required int  RoleId { get; set; }
        public required bool  Active { get; set; }
        public required string  EPF_No { get; set; }
    }
}
