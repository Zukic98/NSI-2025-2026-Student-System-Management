using Faculty.Core.Entities;
using Faculty.Core.Interfaces;
using Faculty.Infrastructure.Db;
using Faculty.Infrastructure.Mappers;

namespace Faculty.Infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly FacultyDbContext _context;

        public CourseRepository(FacultyDbContext context)
        {
            _context = context;
        }

        public Task<Course> AddAsync(Course course)
        {
            course.Id = Guid.NewGuid();
            var schema = CourseMapper.ToPersistence(course);
            _context.Courses.Add(schema);
            return Task.FromResult(course);
        }

        public Task<Course?> GetByIdAsync(Guid id)
        {
            var schema = _context.Courses.FirstOrDefault(x => x.Id == id);
            if (schema == null)
                return Task.FromResult<Course?>(null);
            
            return Task.FromResult<Course?>(CourseMapper.ToDomain(schema, includeRelationships: false));
        }

        public Task<List<Course>> GetAllAsync()
        {
            var schemas = _context.Courses.ToList();
            var domains = CourseMapper.ToDomainCollection(schemas, includeRelationships: false);
            return Task.FromResult(domains.ToList());
        }

        public Task<Course?> UpdateAsync(Course course)
        {
            var existing = _context.Courses.FirstOrDefault(x => x.Id == course.Id);
            if (existing == null)
                return Task.FromResult<Course?>(null);

            CourseMapper.UpdatePersistence(existing, course);
            var updated = CourseMapper.ToDomain(existing, includeRelationships: false);
            return Task.FromResult<Course?>(updated);
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            var existing = _context.Courses.FirstOrDefault(x => x.Id == id);
            if (existing == null)
                return Task.FromResult(false);

            _context.Courses.Remove(existing);
            return Task.FromResult(true);
        }
    }
}
