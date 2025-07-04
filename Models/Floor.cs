using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AMSProj.Models
{
    public class Floor
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public string FloorNo { get; set; } = null!;

        // Foreign key to Campus
        [Required]
        public Guid BuildingID { get; set; }

        [ForeignKey("BuildingID")]
        public Building Building { get; set; } = null!;

        public ICollection<Department> Departments { get; set; } = new List<Department>();
        public ICollection<Lab> Labs { get; set; } = new List<Lab>();
        public ICollection<Classroom> Classrooms { get; set; } = new List<Classroom>();

        public ICollection<Cabin> Cabins { get; set; } = new List<Cabin>();

        public ICollection<Auditorium> Auditoriums { get; set; } = new List<Auditorium>();

        public ICollection<MeetingHall> MeetingHalls { get; set; } = new List<MeetingHall>();

    }
}
