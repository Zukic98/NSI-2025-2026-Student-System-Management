using Microsoft.EntityFrameworkCore;
using Faculty.Core.Entities;

namespace Faculty.Infrastructure.Db
{
    public class FacultyDbContext : DbContext
    {
        public FacultyDbContext(DbContextOptions<FacultyDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
