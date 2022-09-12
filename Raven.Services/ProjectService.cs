using Raven.Core.Interfaces;
using Raven.Core.Models;
using Raven.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProjectService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<Project> AddProject(Project project)
        {
            await _unitOfWork.Projects.AddAsync(project);
            await _unitOfWork.CommitAsync();
            return project;
        }

        public async Task<IEnumerable<Project>> GetAllProjects()
        {
            return await _unitOfWork.Projects.GetAllAsync();
        }

        public async Task<Project> GetProject(Guid projectId)
        {
            return await _unitOfWork.Projects.GetByIdAsync(projectId);
        }

        public async Task UpdateProject(Project oldProj, Project newProj)
        {
            oldProj.Requirements = newProj.Requirements;
            oldProj.Title = newProj.Title;
            oldProj.Info = newProj.Info;
            oldProj.TestCases = newProj.TestCases;
            oldProj.UpdatedDate = DateTime.UtcNow;
            oldProj.ShortCode = newProj.ShortCode;

            await _unitOfWork.CommitAsync();
        }
    }
}
