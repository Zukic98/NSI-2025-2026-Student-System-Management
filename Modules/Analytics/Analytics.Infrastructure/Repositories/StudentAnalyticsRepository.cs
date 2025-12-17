using Analytics.Core.Interfaces;
using Faculty.Infrastructure.Db; // Referenca na tvoj DbContext
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Analytics.Infrastructure.Repositories
{
	public class StudentAnalyticsRepository : IStudentAnalyticsRepository
	{
		private readonly FacultyDbContext _context;

		public StudentAnalyticsRepository(FacultyDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<StudentCourseData>> GetStudentCoursesAsync(string userId, string facultyId)
		{
			// Pronalazimo studenta koristeći UserId iz JWT tokena
			var student = await _context.Students
				.FirstOrDefaultAsync(s => s.UserId == userId);

			if (student == null) return Enumerable.Empty<StudentCourseData>();

			// Izvlačimo sve upise (Enrollments) za tog studenta
			// Enrollment tabela povezuje studenta sa Course tabelom gde su ECTS bodovi
			return await _context.Enrollments
				.Where(e => e.StudentId == student.Id)
				.Select(e => new StudentCourseData
				{
					CourseName = e.Course.Name,
					Grade = e.Grade, // Može biti null ako ispit još nije polagan
					Ects = e.Course.ECTS
				})
				.ToListAsync();
		}
	}
}