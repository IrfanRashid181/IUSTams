using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AMSProj.Models
{
    public class Building
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public string Building_Name { get; set; } = null!;

        [Required]
        public string Type { get; set; } = null!;

        [Required]
        public string Nearest_Building { get; set; } = null!;

        [Required]
        public int NoOfFloors { get; set; }  = 0 ;  // ✅ Default value 0
        
        // Foreign key to Campus
        [Required]
        public Guid CampusID { get; set; }

        [ForeignKey("CampusID")]
        public Campus Campus { get; set; } = null!;

        public ICollection<Floor> Floors { get; set; } = new List<Floor>();

    }
}
