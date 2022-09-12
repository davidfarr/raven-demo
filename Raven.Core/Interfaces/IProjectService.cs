using Raven.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Core.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetAllProjects();
        Task<Project> GetProject(Guid projectId);
        Task<Project> AddProject(Project project);
        Task UpdateProject(Project oldProject, Project newProject);
    }
}
