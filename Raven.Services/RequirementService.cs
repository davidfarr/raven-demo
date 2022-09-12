using Raven.Core.Interfaces;
using Raven.Core.Models;
using Raven.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Services
{
    public class RequirementService : IRequirementService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RequirementService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<Requirement> AddRequirement(Requirement requirement)
        {
            await _unitOfWork.Requirements.AddAsync(requirement);
            await _unitOfWork.CommitAsync();
            return requirement;
        }

        public async Task<IEnumerable<Requirement>> GetAllRequirements()
        {
            return await _unitOfWork.Requirements.GetAllAsync();
        }

        public async Task<IEnumerable<Requirement>> GetAllRequirementsByProject(Guid projectId)
        {
            var allRequirements = await _unitOfWork.Requirements.GetAllAsync();
            allRequirements.Where(x => x.ProjectId == projectId).ToList();
            return allRequirements;
        }

        public async Task<Requirement> GetRequirement(Guid requirementId)
        {
            return await _unitOfWork.Requirements.GetByIdAsync(requirementId);
        }

        public async Task UpdateRequirement(Requirement reqForUpdate, Requirement req)
        {
            reqForUpdate.Title = req.Title;
            reqForUpdate.UpdatedDate = DateTime.UtcNow;
            reqForUpdate.TestCases = req.TestCases;
            reqForUpdate.VersionIntroduced = req.VersionIntroduced;
            reqForUpdate.Info = req.Info;
            reqForUpdate.ProjectId = req.ProjectId;

            await _unitOfWork.CommitAsync();
        }
    }
}
