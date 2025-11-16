using Microsoft.EntityFrameworkCore;
using University.Core.Entities;

namespace University.Infrastructure.Db;

public class UniversityDbContext : DbContext
{
    public UniversityDbContext(DbContextOptions<UniversityDbContext> options)
        : base(options)
    {
    }

    public DbSet<Course> Courses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(builder =>
        {
            builder.ToTable("university_Course");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name).IsRequired();
            builder.Property(c => c.Code).IsRequired();
            builder.Property(c => c.Ects).IsRequired();
            builder.Property(c => c.AcademicYear).IsRequired();
            builder.Property(c => c.Semester).IsRequired();
            builder.Property(c => c.ProfessorId).IsRequired();
            builder.Property(c => c.FacultyId).IsRequired();
        });
    }
}
