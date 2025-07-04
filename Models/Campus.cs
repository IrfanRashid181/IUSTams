using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMSProj.Models
{
    public class Campus
    {
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Location { get; set; } = null!;

        [Required]
        public string Area { get; set; } = null!;// varchar equivalent

        [Required]
        public int NoOfBuildings { get; set; } = 0;  // ✅ Default value 0

        public ICollection<Building> Buildings { get; set; } = new List<Building>();

    }
}

