using Analytics.Application.DTOs;
using Analytics.Core.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Analytics.Application.Services
{
	public class StudentAnalyticsService : IStudentAnalyticsService
	{
		private readonly IStudentAnalyticsRepository _repository;

		public StudentAnalyticsService(IStudentAnalyticsRepository repository)
		{
			_repository = repository;
		}

		public async Task<StudentStatsDto> GetStudentStatsAsync(string userId, string facultyId)
		{
			// 1. Pozovi repozitorij da dobiješ sirove podatke
			var rawData = (await _repository.GetStudentCoursesAsync(userId, facultyId)).ToList();

			// 2. Filtriraj položene (ocjena >= 6)
			var passed = rawData.Where(c => c.Grade.HasValue && c.Grade.Value >= 6).ToList();

			// 3. GPA kalkulacija
			double totalPoints = passed.Sum(c => c.Grade.Value * c.Ects);
			int passedEcts = passed.Sum(c => c.Ects);
			double gpa = passedEcts > 0 ? totalPoints / passedEcts : 0;

			// 4. Completion kalkulacija
			double completion = rawData.Any()
				? (double)passed.Count / rawData.Count * 100
				: 0;

			// 5. Mapiraj u DTO
			return new StudentStatsDto
			{
				Gpa = Math.Round(gpa, 2),
				TotalECTS = passedEcts,
				RequiredECTS = rawData.Sum(c => c.Ects),
				SemesterCompletion = Math.Round(completion, 2),
				GradeDistribution = rawData.Select(c => new GradeEntryDto
				{
					CourseName = c.CourseName,
					Grade = c.Grade ?? 5 // Ako nema ocjene, prikaži 5 (nije položen)
				}).ToList()
			};
		}
	}
}