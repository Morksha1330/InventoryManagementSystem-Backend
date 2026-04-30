namespace InventoryMgtSystem.DTO
{
    public class LoginDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }


    }

    public class ChangePassword 
    {
        public int UserId { get; set; }
        public string? CurrentPassword { get; set; }

        public string? NewPassword { get; set; }
    }

    public class customUser
    {
        public string? Username { get; set; }
    }
}
