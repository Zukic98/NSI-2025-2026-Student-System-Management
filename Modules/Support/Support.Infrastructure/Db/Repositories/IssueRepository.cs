using Common.Infrastructure.Repositories;
using Support.Core.Entities;
using Support.Core.Interfaces.Repositories;

namespace Support.Infrastructure.Db.Repositories
{
    public class IssueRepository : BaseRepository<Issue>, IIssueRepository
    {
        public IssueRepository(SupportDbContext context)
            : base(context) { }
    }
}
