using MediatR;
using University.Application.DTOs;
using University.Application.Events;
using University.Core.Entities;
using University.Core.Interfaces;

public class CourseService
{
    private readonly ICourseRepository _repo;
    private readonly IMediator _mediator;

    public CourseService(ICourseRepository repo, IMediator mediator)
    {
        _repo = repo;
        _mediator = mediator;
    }

    // CREATE
    public async Task<Course> CreateAsync(CreateCourseDto dto)
    {
        var course = new Course
        {
            Id = Guid.NewGuid(),
            FacultyId = dto.FacultyId,
            Name = dto.Name,
            Code = dto.Code,
            Ects = dto.Ects,
            AcademicYear = dto.AcademicYear,
            Semester = dto.Semester,
            ProfessorId = dto.ProfessorId
        };

        await _repo.AddAsync(course);

        // Publish event
        await _mediator.Publish(new CourseCreatedEvent
        {
            CourseId = course.Id,
            FacultyId = course.FacultyId,
            Name = course.Name,
            Code = course.Code,
            Ects = course.Ects,
            AcademicYear = course.AcademicYear,
            Semester = course.Semester,
            ProfessorId = course.ProfessorId
        });

        return course;
    }

    // READ
    public Task<List<Course>> GetAllAsync() => _repo.GetAllAsync();

    public Task<Course?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);

    // UPDATE
    public async Task<Course?> UpdateAsync(Guid id, UpdateCourseDto dto)
    {
        var course = await _repo.GetByIdAsync(id);
        if (course == null) return null;

        course.Name = dto.Name;
        course.Code = dto.Code;
        course.Ects = dto.Ects;
        course.AcademicYear = dto.AcademicYear;
        course.Semester = dto.Semester;
        course.ProfessorId = dto.ProfessorId;

        await _repo.UpdateAsync(course);

        // Publish update event
        await _mediator.Publish(new CourseUpdatedEvent
        {
            CourseId = course.Id,
            FacultyId = course.FacultyId
        });

        return course;
    }

    // DELETE
    public async Task<bool> DeleteAsync(Guid id)
    {
        var course = await _repo.GetByIdAsync(id);
        if (course == null) return false;

        var success = await _repo.DeleteAsync(id);

        if (success)
        {
            await _mediator.Publish(new CourseDeletedEvent
            {
                CourseId = course.Id,
                FacultyId = course.FacultyId
            });
        }

        return success;
    }
}
