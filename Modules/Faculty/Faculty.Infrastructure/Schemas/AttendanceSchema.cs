namespace Faculty.Infrastructure.Schemas;

/// <summary>
/// Persistence schema for Attendance entity.
/// </summary>
public class AttendanceSchema
{
    public int Id { get; set; }
    public Guid FacultyId { get; set; }
    public int StudentId { get; set; }
    public Guid CourseId { get; set; }
    public DateTime LectureDate { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public StudentSchema Student { get; set; } = null!;
    public CourseSchema Course { get; set; } = null!;
}

