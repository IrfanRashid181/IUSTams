using System.ComponentModel.DataAnnotations;

namespace AMSProj.Models
{
    public class Facility
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = null!;

        public ICollection<ClassroomFacility> ClassroomFacilities { get; set; } = new List<ClassroomFacility>();

    }
}
