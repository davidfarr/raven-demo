using Raven.Core.Models;
using Raven.Core.Repositories;

namespace Raven.Data.Repositories
{
    public class RequirementRepository : Repository<Requirement>, IRequirementRepository
    {
        public RequirementRepository(RavenDbContext context)
            : base(context)
        {

        }

        private RavenDbContext RavenDbContext
        {
            get { return Context as RavenDbContext; }
        }
    }
}
