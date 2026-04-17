namespace InventoryMgtSystem.Models
{
    public class LoginResponseModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? AccessToken { get; internal set; }
        public int ExpiresIn { get; internal set; }

    }
}
