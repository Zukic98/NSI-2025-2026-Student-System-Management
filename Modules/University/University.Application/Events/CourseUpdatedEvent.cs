using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Events;

public class CourseUpdatedEvent : INotification
{
    public Guid CourseId { get; set; }
    public Guid FacultyId { get; set; }

    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public int Ects { get; set; }
    public string AcademicYear { get; set; } = default!;
    public string Semester { get; set; } = default!;
    public Guid ProfessorId { get; set; }
}



