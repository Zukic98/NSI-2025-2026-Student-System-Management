using Common.Infrastructure.Repositories;
using Support.Core.Entities;
using Support.Core.Interfaces.Repositories;

namespace Support.Infrastructure.Db.Repositories
{
    public class IssueCategoryRepository : BaseRepository<IssueCategory>, IIssueCategoryRepository
    {
        public IssueCategoryRepository(SupportDbContext context)
            : base(context) { }
    }
}
