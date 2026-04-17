namespace InventoryMgtSystem.DTO
{
    public class RegisterDto
    {
        public required string Name { get; set; }
        public required string Username { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public required string  Role { get; set; }
        public required bool  Active { get; set; }
        public required string  EPF_No { get; set; }
    }
}
