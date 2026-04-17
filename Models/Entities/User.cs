//using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
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
        public int Role { get; set; }
        public bool Active { get; set; }
        public int? InitialAttempt { get; set; }
        [Required]
        public string EPF_No { get; set; }


    }


}