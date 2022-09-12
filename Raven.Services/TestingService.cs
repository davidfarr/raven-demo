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
    public class TestingService : ITestingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TestingService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<TestCase> AddTestCase(TestCase testCase)
        {
            await _unitOfWork.TestCases.AddAsync(testCase);
            await _unitOfWork.CommitAsync();
            return testCase;
        }

        public async Task<IEnumerable<TestCase>> GetAllTestCases()
        {
            return await _unitOfWork.TestCases.GetAllAsync();
        }

        public async Task<IEnumerable<TestCase>> GetAllTestCasesByProject(Guid projectId)
        {
            var allTcs = await _unitOfWork.TestCases.GetAllAsync();
            allTcs.Where(x => x.ProjectId == projectId).ToList();
            return allTcs;
        }

        public async Task<TestCase> GetTestCase(Guid testCaseId)
        {
            return await _unitOfWork.TestCases.GetByIdAsync(testCaseId);
        }

        public async Task UpdateTestCase(TestCase oldTc, TestCase newTc)
        {
            oldTc.UpdatedDate = DateTime.UtcNow;
            oldTc.Project = newTc.Project;
            oldTc.Title = newTc.Title;
            oldTc.Info = newTc.Info;
            oldTc.Requirement = newTc.Requirement;

            await _unitOfWork.CommitAsync();
        }
    }
}
