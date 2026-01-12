using Common.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using University.Core.Entities;
using University.Core.Interfaces;
using University.Infrastructure.Db;

namespace University.Infrastructure.Repositories
{
    public class AcademicYearRepository : BaseRepository<AcademicYear>, IAcademicYearRepository
    {
        private new readonly UniversityDbContext _context;

        public AcademicYearRepository(UniversityDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<AcademicYear?> GetActiveAcademicYearAsync()
        {
            var entity = await _context.AcademicYears.FirstOrDefaultAsync(ay => ay.IsActive);

            if (entity == null)
                return null;

            return new AcademicYear
            {
                Id = entity.Id,
                Year = entity.Year,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                IsActive = entity.IsActive,
            };
        }
    }
}
