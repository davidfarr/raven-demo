using Raven.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Raven.Data
{
    public partial class TestCase : IProjectDatum
    {
        public TestCase()
        {
            TestCaseSteps = new HashSet<TestCaseStep>();
        }

        public Guid TestCaseId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid RequirementId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Title { get; set; }
        public string Info { get; set; }

        public virtual Project Project { get; set; }
        public virtual Requirement Requirement { get; set; }
        public virtual ICollection<TestCaseStep> TestCaseSteps { get; set; }
    }
}
