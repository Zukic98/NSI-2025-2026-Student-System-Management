using Microsoft.EntityFrameworkCore;
using University.Core.Entities;
using University.Core.Interfaces;
using University.Infrastructure.Db;

namespace University.Infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly UniversityDbContext _db;

        public CourseRepository(UniversityDbContext db)
        {
            _db = db;
        }

        public async Task<Course> AddAsync(Course course)
        {
            _db.Courses.Add(course);
            await _db.SaveChangesAsync();
            return course;
        }

        public async Task<List<Course>> GetAllAsync()
        {
            return await _db.Courses.ToListAsync();
        }

        public async Task<Course?> GetByIdAsync(Guid id)
        {
            return await _db.Courses.FindAsync(id);
        }

        public async Task<Course?> UpdateAsync(Course course)
        {
            _db.Courses.Update(course);
            await _db.SaveChangesAsync();
            return course;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _db.Courses.FindAsync(id);
            if (entity == null) return false;

            _db.Courses.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
