using System.ComponentModel.DataAnnotations;
namespace GreenMonitor.Models
{
    public class LoginUser
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? HashedPassword { get; set; }
    }
}