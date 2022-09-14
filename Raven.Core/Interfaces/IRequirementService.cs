using Raven.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Raven.Core.Interfaces
{
    public interface IRequirementService
    {
        Task<IEnumerable<Requirement>> GetAllRequirements();
        Task<Requirement> GetRequirement(Guid requirementId);
        Task<IEnumerable<Requirement>> GetAllRequirementsByProject(Guid projectId);
        Task<Requirement> AddRequirement(Requirement requirement);
        Task UpdateRequirement(Requirement oldReq, Requirement newReq);
    }
}
