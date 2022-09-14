using System;
using System.Threading.Tasks;

namespace Raven.Core.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IProjectRepository Projects { get; }
        ITestingRepository TestCases { get; }
        IRequirementRepository Requirements { get; }
        Task<int> CommitAsync();
    }
}
