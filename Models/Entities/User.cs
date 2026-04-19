//using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InventoryMgtSystem.Models.Entities
{
    public class User : BaseEntity
    {
        [Required]
        public  string Name { get; set; }
        [Required]
        public  string Username { get; set; }
        [Required]
        public  string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public int RoleId { get; set; }
        public bool Active { get; set; }
        public int? InitialAttempt { get; set; }
        [Required]
        public string EPF_No { get; set; }
        public string? PhoneNo { get; set; }

        // Navigation property
        [JsonIgnore]
        public virtual Role Role { get; set; }
    }


}