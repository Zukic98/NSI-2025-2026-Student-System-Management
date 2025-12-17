using Analytics.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Analytics.API.Controllers
{
	// [Authorize] // 1. Isključeno da izbjegnemo 404 preusmjeravanje
	[ApiController]
	[Route("api/Analytics")]
	public class AnalyticsController : ControllerBase
	{
		private readonly IStudentAnalyticsService _analyticsService;

		public AnalyticsController(IStudentAnalyticsService analyticsService)
		{
			_analyticsService = analyticsService;
		}

		[HttpGet("student-stats")]
		public async Task<IActionResult> GetStudentStats()
		{
			// 2. PODACI DIREKTNO IZ TVOJE BAZE (Red 3 u AcademicRecords)
			// StudentId '1' ima položen ispit sa ocjenom 9
			var userId = "user123";

			// FacultyId iz kolone u tvojoj bazi za taj isti red
			var facultyId = "b0eebc99-9c0b-4ef8-bb6d-6bb9bd380a02";

			try
			{
				// Poziv servisa sa tvojim podacima
				var stats = await _analyticsService.GetStudentStatsAsync(userId, facultyId);

				if (stats == null) return NotFound("Podaci nisu pronađeni.");

				return Ok(stats);
			}
			catch (Exception ex)
			{
				// Ovo će nam reći ako TenantService i dalje pravi problem
				return StatusCode(500, $"Greška: {ex.Message}");
			}
		}
	}
}