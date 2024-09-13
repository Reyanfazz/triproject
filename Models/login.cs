using System.ComponentModel.DataAnnotations;

namespace certificate.Models
{
    public class login
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}
