using Raven.Core.Repositories;
using Raven.Data.Repositories;
using System.Threading.Tasks;

namespace Raven.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RavenDbContext _context;
        private ProjectRepository _projectRepository;
        private TestingRepository _testingRepository;
        private RequirementRepository _requirementRespository;

        public UnitOfWork(RavenDbContext context)
        {
            this._context = context;
        }

        public IProjectRepository Projects => _projectRepository = _projectRepository ?? new ProjectRepository(_context);
        public ITestingRepository TestCases => _testingRepository = _testingRepository ?? new TestingRepository(_context);
        public IRequirementRepository Requirements => _requirementRespository = _requirementRespository ?? new RequirementRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
