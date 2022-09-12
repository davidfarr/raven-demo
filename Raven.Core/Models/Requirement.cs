using Raven.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Raven.Core.Models
{
    public partial class Requirement : IProjectDatum
    {
        public Requirement()
        {
            TestCases = new HashSet<TestCase>();
        }

        public Guid RequirementId { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Title { get; set; }
        public string Info { get; set; }
        public string VersionIntroduced { get; set; }

        public virtual Project Project { get; set; }
        public virtual ICollection<TestCase> TestCases { get; set; }
    }
}
