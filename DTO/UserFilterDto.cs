namespace InventoryMgtSystem.DTO;

public class UserFilterDto
{
    public string? SearchTerm { get; set; }
    public int? RoleId { get; set; }
    public bool? Active { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; } = "Id";
    public string? SortOrder { get; set; } = "ASC"; // ASC or DESC
}