using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AMSProj.Models
{
    public class Auditorium
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public string Auditorium_Name { get; set; } = null!;


        // Foreign key to Floor
        [Required]
        public Guid FloorID { get; set; }

        [ForeignKey("FloorID")]
        public Floor Floor { get; set; } = null!;

        //public ICollection<ClassroomFacility> ClassroomFacilities { get; set; } = new List<ClassroomFacility>();
    }
}
