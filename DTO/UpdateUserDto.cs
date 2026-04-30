namespace InventoryMgtSystem.DTO;

public class UpdateUserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string? EPF_No { get; set; }
    public int? RoleId { get; set; }
    public bool? Active { get; set; }
    public string? Password { get; set; }
}