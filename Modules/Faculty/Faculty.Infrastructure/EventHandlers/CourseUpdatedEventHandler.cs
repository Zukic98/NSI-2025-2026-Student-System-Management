using MediatR;
using Microsoft.EntityFrameworkCore;
using University.Application.Events;
using Faculty.Infrastructure.Db;
using Faculty.Core.Entities;

namespace Faculty.Infrastructure.EventHandlers
{
    public class CourseUpdatedEventHandler : INotificationHandler<CourseUpdatedEvent>
    {
        private readonly FacultyDbContext _db;

        public CourseUpdatedEventHandler(FacultyDbContext db)
        {
            _db = db;
        }

        public async Task Handle(CourseUpdatedEvent notification, CancellationToken cancellationToken)
        {
            var entity = await _db.FacultyCourses
                .FirstOrDefaultAsync(c => c.CourseIdFromUniversity == notification.CourseId, cancellationToken);

            if (entity == null)
            {
                Console.WriteLine($"[Faculty] UPDATED event received, but CourseId={notification.CourseId} not found.");
                return;
            }

            entity.Name = notification.Name;
            entity.Code = notification.Code;
            entity.Ects = notification.Ects;
            entity.AcademicYear = notification.AcademicYear;
            entity.Semester = notification.Semester;
            entity.ProfessorId = notification.ProfessorId;

            await _db.SaveChangesAsync(cancellationToken);

            Console.WriteLine($"[Faculty] Course UPDATED replicated successfully → {entity.Name}");
        }
    }
}
