using System;
using System.Collections.Generic;
using System.Text;

namespace Raven.Core.Interfaces
{
    public interface IProjectDatum
    {
        Guid ProjectId { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime? UpdatedDate { get; set; }
    }
}
