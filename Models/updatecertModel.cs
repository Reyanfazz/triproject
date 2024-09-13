using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace certificate.Models
{
    public class updatecertModel
    {

        [Key]
        [Required]
        public string SId { get; set; }

        [Required]
        public string SName { get; set; }

        [NotMapped]
        public IFormFile Scertificate{ get; set; }
    }
}
