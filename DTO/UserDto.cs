namespace InventoryMgtSystem.DTO;

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string EPF_No { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; }
    public bool Active { get; set; }
    public int? InitialAttempt { get; set; }
    public int? CreatedUser { get; set; }
    public DateTime? CreatedDate { get; set; }
}