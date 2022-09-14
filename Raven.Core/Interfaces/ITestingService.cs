using Raven.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Raven.Core.Interfaces
{
    public interface ITestingService
    {
        Task<IEnumerable<TestCase>> GetAllTestCases();
        Task<TestCase> GetTestCase(Guid testCaseId);
        Task<IEnumerable<TestCase>> GetAllTestCasesByProject(Guid projectId);
        Task<TestCase> AddTestCase(TestCase testCase);
        Task UpdateTestCase(TestCase oldTc, TestCase newTc);
    }
}
