using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AMSProj.Models
{
    public class ClassroomFacility
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public string Quantity { get; set; } = null!;

        // Foreign key to Department
        [Required]
        public Guid FacilityID { get; set; }

        [ForeignKey("FacilityID")]
        public Facility Facility { get; set; } = null!;

        // Foreign key to Floor
        [Required]
        public Guid ClassroomID { get; set; }

        [ForeignKey("ClassroomID")]
        public Classroom Classroom { get; set; } = null!;

    }
}
