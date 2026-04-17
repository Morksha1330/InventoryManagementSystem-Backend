namespace InventoryMgtSystem.DTO
{
    public class LoginDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }


    }

    public class ChangePassword : LoginDto
    {
        public string? newPassword { get; set; }
        public string? ConfirmPassword { get; set; }
    }

    public class customUser
    {
        public string? Username { get; set; }
    }
}
