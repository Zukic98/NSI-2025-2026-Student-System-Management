using University.Application.DTOs;
using University.Core.Entities;
using University.Core.Interfaces;

public class CourseService
{
    private readonly ICourseRepository _repo;

    public CourseService(ICourseRepository repo)
    {
        _repo = repo;
    }

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

        return await _repo.AddAsync(course);
    }

    public Task<List<Course>> GetAllAsync() => _repo.GetAllAsync();

    public Task<Course?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);

    public async Task<Course?> UpdateAsync(Guid id, UpdateCourseDto dto)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return null;

        existing.Name = dto.Name;
        existing.Code = dto.Code;
        existing.Ects = dto.Ects;
        existing.AcademicYear = dto.AcademicYear;
        existing.Semester = dto.Semester;
        existing.ProfessorId = dto.ProfessorId;

        return await _repo.UpdateAsync(existing);
    }

    public Task<bool> DeleteAsync(Guid id) => _repo.DeleteAsync(id);
}
