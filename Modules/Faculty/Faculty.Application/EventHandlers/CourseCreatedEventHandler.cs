using MediatR;
using University.Application.Events;

namespace Faculty.Application.EventHandlers;

public class CourseCreatedEventHandler : INotificationHandler<CourseCreatedEvent>
{
    public Task Handle(CourseCreatedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Faculty received CourseCreatedEvent: {notification.Name}");
        return Task.CompletedTask;
    }
}
