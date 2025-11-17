using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Events;

public class CourseDeletedEvent : INotification
{
    public Guid CourseId { get; set; }
    public Guid FacultyId { get; set; }
}
