using MediatR;
using Microsoft.EntityFrameworkCore;
using University.Application.Events;
using Faculty.Infrastructure.Db;

namespace Faculty.Infrastructure.EventHandlers
{
    public class CourseDeletedEventHandler : INotificationHandler<CourseDeletedEvent>
    {
        private readonly FacultyDbContext _db;

        public CourseDeletedEventHandler(FacultyDbContext db)
        {
            _db = db;
        }

        public async Task Handle(CourseDeletedEvent notification, CancellationToken cancellationToken)
        {
            var entity = await _db.FacultyCourses
                .FirstOrDefaultAsync(c => c.CourseIdFromUniversity == notification.CourseId, cancellationToken);

            if (entity == null)
            {
                Console.WriteLine($"[Faculty] DELETE event received, but CourseId={notification.CourseId} not found.");
                return;
            }

            _db.FacultyCourses.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);

            Console.WriteLine($"[Faculty] Course DELETED replicated successfully → CourseIdFromUniversity={notification.CourseId}");
        }
    }
}
