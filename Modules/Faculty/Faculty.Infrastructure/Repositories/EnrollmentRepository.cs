using Faculty.Core.Entities;
using Faculty.Core.Interfaces;
using Faculty.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Faculty.Infrastructure.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly FacultyDbContext _context;

        public EnrollmentRepository(FacultyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Enrollment>> GetByStudentIdAsync(int studentId)
        {
            return await _context.Enrollments
                .AsNoTracking()
                .Include(e => e.Course)
                .Where(e => e.StudentId == studentId)
                .ToListAsync();
        }

        public Task<bool> StudentExistsAsync(int studentId)
            => _context.Students.AnyAsync(s => s.Id == studentId);

        public Task<Course?> GetCourseByIdAsync(Guid courseId)
            => _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);

        public Task<bool> IsAlreadyEnrolledAsync(int studentId, Guid courseId)
            => _context.Enrollments.AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);

        public Task<Enrollment> AddAsync(Enrollment enrollment)
        {
            enrollment.Id = 0; // ako je int identity (ValueGeneratedOnAdd)
            _context.Enrollments.Add(enrollment);
            return Task.FromResult(enrollment);
        }

        public Task SaveChangesAsync()
            => _context.SaveChangesAsync();
    }
}
