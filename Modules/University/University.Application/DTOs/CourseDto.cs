using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.DTOs
{
    public record CreateCourseDto(
        Guid FacultyId,
        string Name,
        string Code,
        int Ects,
        string AcademicYear,
        string Semester,
        Guid ProfessorId
    );

    public record UpdateCourseDto(
        string Name,
        string Code,
        int Ects,
        string AcademicYear,
        string Semester,
        Guid ProfessorId
    );
}
