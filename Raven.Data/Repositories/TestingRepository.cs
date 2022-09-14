using Raven.Core.Models;
using Raven.Core.Repositories;

namespace Raven.Data.Repositories
{
    public class TestingRepository : Repository<TestCase>, ITestingRepository
    {
        public TestingRepository(RavenDbContext context)
            : base(context)
        {

        }

        private RavenDbContext RavenDbContext
        {
            get { return Context as RavenDbContext; }
        }
    }
}
