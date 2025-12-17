using Analytics.Application.DTOs;
using System.Threading.Tasks;

namespace Analytics.Application.Services
{
	public interface IStudentAnalyticsService
	{
		// Metoda koja vraća finalni DTO sa svim proračunima
		Task<StudentStatsDto> GetStudentStatsAsync(string userId, string facultyId);
	}
}