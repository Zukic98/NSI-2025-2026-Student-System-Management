using System.Security.Claims;
using Faculty.Application.DTOs;
using Faculty.Application.Interfaces;
using Faculty.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Faculty.Application.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _repo;
        private readonly IHttpContextAccessor _http;

        public EnrollmentService(IEnrollmentRepository repo, IHttpContextAccessor http)
        {
            _repo = repo;
            _http = http;
        }

        private int GetStudentIdFromJwt()
        {
            var user = _http.HttpContext?.User;
            if (user is null)
                throw new UnauthorizedAccessException("No user context.");

            var idStr =
                user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? user.FindFirst("sub")?.Value
                ?? user.FindFirst("studentId")?.Value;

            if (!int.TryParse(idStr, out var studentId))
                throw new UnauthorizedAccessException("StudentId claim missing/invalid in JWT.");

            return studentId;
        }

        public async Task<List<EnrollmentListItemDto>> GetMyEnrollmentsAsync()
        {
            var studentId = GetStudentIdFromJwt();

            var enrollments = await _repo.GetByStudentIdAsync(studentId);

            return enrollments.Select(e => new EnrollmentListItemDto
            {
                EnrollmentId = e.Id,
                CourseId = e.CourseId,
                CourseName = e.Course?.Name ?? "",
                Status = e.Status,
                Grade = e.Grade
            }).ToList();
        }

        public async Task<CreateEnrollmentResponseDto> CreateEnrollmentAsync(CreateEnrollmentRequestDto dto)
        {
            var studentId = GetStudentIdFromJwt();

            // student exists
            var studentExists = await _repo.StudentExistsAsync(studentId);
            if (!studentExists)
                throw new KeyNotFoundException("Student not found.");

            // course exists
            var course = await _repo.GetCourseByIdAsync(dto.CourseId);
            if (course is null)
                throw new KeyNotFoundException("Course not found.");

            // not already enrolled
            var already = await _repo.IsAlreadyEnrolledAsync(studentId, dto.CourseId);
            if (already)
                throw new InvalidOperationException("Already enrolled in this course");

            var enrollment = new Faculty.Core.Entities.Enrollment
            {
                StudentId = studentId,
                CourseId = dto.CourseId,
                Status = "Enrolled",
                Grade = null,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(enrollment);
            await _repo.SaveChangesAsync();

            return new CreateEnrollmentResponseDto
            {
                EnrollmentId = enrollment.Id,
                StudentId = enrollment.StudentId,
                CourseId = enrollment.CourseId,
                CourseName = course.Name,
                EnrollmentDate = enrollment.CreatedAt
            };
        }
    }
}
