using Raven.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Raven.Data
{
    public partial class Project : IProjectDatum
    {
        public Project()
        {
            Requirements = new HashSet<Requirement>();
            TestCaseSteps = new HashSet<TestCaseStep>();
            TestCases = new HashSet<TestCase>();
        }

        public Guid ProjectId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Title { get; set; }
        public string Info { get; set; }
        public string ShortCode { get; set; }
        public string ImageLocation { get; set; }

        public virtual ICollection<Requirement> Requirements { get; set; }
        public virtual ICollection<TestCaseStep> TestCaseSteps { get; set; }
        public virtual ICollection<TestCase> TestCases { get; set; }
    }
}
