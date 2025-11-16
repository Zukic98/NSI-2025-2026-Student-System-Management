using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Core.Entities;


namespace University.Core.Interfaces
{
    public interface ICourseRepository
    {
        Task<Course> AddAsync(Course course);
        Task<Course?> GetByIdAsync(Guid id);
        Task<List<Course>> GetAllAsync();
        Task<Course?> UpdateAsync(Course course);
        Task<bool> DeleteAsync(Guid id);
    }
}
