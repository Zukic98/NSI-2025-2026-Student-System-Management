using Microsoft.EntityFrameworkCore;
using Support.Core.Entities;

namespace Support.Infrastructure.Db
{
<<<<<<< HEAD
    public class SupportDbContext : DbContext
    {
        public SupportDbContext(DbContextOptions<SupportDbContext> options)
            : base(options)
        {
        }

        public DbSet<Request> Requests { get; set; }
    }
=======
	public class SupportDbContext(DbContextOptions<SupportDbContext> options) : DbContext(options)
	{
		public DbSet<DocumentRequest> DocumentRequests { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(SupportDbContext).Assembly);

			base.OnModelCreating(modelBuilder);
		}
	}
>>>>>>> 28ad086ec194b0f4e021dae55008bf0e637ee75d
}
