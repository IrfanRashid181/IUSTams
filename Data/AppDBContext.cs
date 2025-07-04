using Microsoft.EntityFrameworkCore;
using AMSProj.Models;

namespace AMSProj.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        public DbSet<Campus> Campuses { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Lab> Labs { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Cabin> Cabins { get; set; }

        public DbSet<Auditorium> Auditoriums { get; set; }

        public DbSet<MeetingHall> MeetingHalls { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<ClassroomFacility> ClassroomFacilities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Set default values
            modelBuilder.Entity<Building>()
                .Property(b => b.NoOfFloors)
                .HasDefaultValue(0);

            modelBuilder.Entity<Campus>()
                .Property(c => c.NoOfBuildings)
                .HasDefaultValue(0);

            // Relationships - Campus & Building
            modelBuilder.Entity<Building>()
                .HasOne(b => b.Campus)
                .WithMany(c => c.Buildings)
                .HasForeignKey(b => b.CampusID)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationships - Building & Floor
            modelBuilder.Entity<Floor>()
                .HasOne(f => f.Building)
                .WithMany(b => b.Floors)
                .HasForeignKey(f => f.BuildingID)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationships - Floor & Department
            modelBuilder.Entity<Department>()
                .HasOne(d => d.Floor)
                .WithMany(f => f.Departments)
                .HasForeignKey(d => d.FloorID)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationships - Floor & Auditorium
            modelBuilder.Entity<Auditorium>()
                .HasOne(d => d.Floor)
                .WithMany(f => f.Auditoriums)
                .HasForeignKey(d => d.FloorID)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationships - Floor & MeetingHall
            modelBuilder.Entity<MeetingHall>()
                .HasOne(d => d.Floor)
                .WithMany(f => f.MeetingHalls)
                .HasForeignKey(d => d.FloorID)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationships - Lab
            modelBuilder.Entity<Lab>()
                .HasOne(l => l.Department)
                .WithMany(d => d.Labs)
                .HasForeignKey(l => l.DepartmentID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Lab>()
                .HasOne(l => l.Floor)
                .WithMany(f => f.Labs)
                .HasForeignKey(l => l.FloorID)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationships - Classroom
            modelBuilder.Entity<Classroom>()
                .HasOne(c => c.Department)
                .WithMany(d => d.Classrooms)
                .HasForeignKey(c => c.DepartmentID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Classroom>()
                .HasOne(c => c.Floor)
                .WithMany(f => f.Classrooms)
                .HasForeignKey(c => c.FloorID)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationships - Cabin
            modelBuilder.Entity<Cabin>()
                .HasOne(c => c.Department)
                .WithMany(d => d.Cabins)
                .HasForeignKey(c => c.DepartmentID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cabin>()
                .HasOne(c => c.Floor)
                .WithMany(f => f.Cabins)
                .HasForeignKey(c => c.FloorID)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationships - ClassroomFacility
            modelBuilder.Entity<ClassroomFacility>()
                .HasOne(cf => cf.Classroom)
                .WithMany(c => c.ClassroomFacilities)
                .HasForeignKey(cf => cf.ClassroomID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClassroomFacility>()
                .HasOne(cf => cf.Facility)
                .WithMany(f => f.ClassroomFacilities)
                .HasForeignKey(cf => cf.FacilityID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
