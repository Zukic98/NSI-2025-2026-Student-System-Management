using System.Collections.Generic;
using System.Threading.Tasks;

namespace Analytics.Core.Interfaces
{
	public interface IStudentAnalyticsRepository
	{
		/// <summary>
		/// Dobavlja sve kurseve, ocjene i ECTS bodove za određenog studenta.
		/// </summary>
		Task<IEnumerable<StudentCourseData>> GetStudentCoursesAsync(string userId, string facultyId);
	}

	// Ova klasa služi kao "sirovi" prenosnik podataka iz baze 
	// pre nego što servis izračuna prosek i procente.
	public class StudentCourseData
	{
		public string CourseName { get; set; } = string.Empty;
		public int? Grade { get; set; } // Nullable jer u Enrollment tabeli Grade može biti null
		public int Ects { get; set; }

		// Pomoćna svojstva koja će olakšati računanje u servisu
		public bool IsPassed => Grade.HasValue && Grade.Value >= 6;
		public bool IsCompleted => Grade.HasValue;
	}
}