using System;
using System.ComponentModel.DataAnnotations;

namespace certificate.Models
{
    public class student
    {
        [Key]
        [Required]
        public string SId { get; set; }

        [Required]
        public string SName { get; set; }

        [Required]
        public DateTime Sdob { get; set; }

        [Required]
        [Phone]
        public string mobileNo { get; set; }

        [Required]
        [EmailAddress]
        public string SEmail { get; set; }

        [Required]
        public string SAddress { get; set; }

        public string Scertificate { get; set; }

        public string Scourse { get; set; }
        public string Sgender { get; set; }
    }
}
