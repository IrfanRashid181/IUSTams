using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AMSProj.Models
{
    public class Department
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public string Department_Name { get; set; } = null!;

        // Foreign key to Campus
        [Required]
        public Guid FloorID { get; set; }

        [ForeignKey("FloorID")]
        public Floor Floor { get; set; } = null!;

        public ICollection<Lab> Labs { get; set; } = new List<Lab>();

        public ICollection<Classroom> Classrooms { get; set; } = new List<Classroom>();

        public ICollection<Cabin> Cabins { get; set; } = new List<Cabin>();

    }
}
