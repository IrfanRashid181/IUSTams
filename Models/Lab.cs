using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AMSProj.Models
{
    public class Lab
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public string Lab_Name { get; set; } = null!;

        // Foreign key to Department
        [Required]
        public Guid DepartmentID { get; set; }

        [ForeignKey("DepartmentID")]
        public Department Department { get; set; } = null!;

        // Foreign key to Floor
        [Required]
        public Guid FloorID { get; set; }

        [ForeignKey("FloorID")]
        public Floor Floor { get; set; } = null!;
    }
}
