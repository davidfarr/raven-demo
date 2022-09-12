using System;
using System.Collections.Generic;

namespace Raven.Core.Models
{
    public partial class TestCaseStep
    {
        public Guid TestCaseStepId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid TestCaseId { get; set; }
        public string Step { get; set; }

        public virtual Project Project { get; set; }
        public virtual TestCase TestCase { get; set; }
    }
}
