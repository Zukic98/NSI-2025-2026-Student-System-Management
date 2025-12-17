using System;
using System.Collections.Generic;

namespace Analytics.Application.DTOs
{
	public class StudentStatsDto
	{
		// GPA (Prosek) - kalkulacija: SUM(Grade * ECTS) / SUM(ECTS)
		public double Gpa { get; set; }

		// Total ECTS - suma ECTS-a za položene kurseve (ocena >= 6)
		public int TotalECTS { get; set; }

		// Required ECTS - suma ECTS-a za sve kurseve studenta u semestru
		public int RequiredECTS { get; set; }

		// Semester Completion - procenat (položeni / ukupni kursevi) * 100
		public double SemesterCompletion { get; set; }

		// Grade Distribution - lista objekata sa nazivom kursa i ocenom
		public List<GradeEntryDto> GradeDistribution { get; set; } = new();
	}

	// Pomoćna klasa za prikaz pojedinačnih ocena
	public class GradeEntryDto
	{
		public string CourseName { get; set; } = string.Empty;
		public int Grade { get; set; }
	}
}