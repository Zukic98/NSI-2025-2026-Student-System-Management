
using Faculty.Core.Entities;

namespace Faculty.Core.Interfaces
{
    public interface IEnrollmentRepository
    {
        Task<List<Enrollment>> GetByStudentIdAsync(int studentId);
        Task<bool> StudentExistsAsync(int studentId);
        Task<Course?> GetCourseByIdAsync(Guid courseId);
        Task<bool> IsAlreadyEnrolledAsync(int studentId, Guid courseId);

        Task<Enrollment> AddAsync(Enrollment enrollment);
        Task SaveChangesAsync();
    }
}
