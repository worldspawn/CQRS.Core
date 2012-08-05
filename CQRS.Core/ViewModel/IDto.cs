using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQRS.Core.ViewModel
{
    public interface IDto
    {
        
    }

    public interface IPersistedDto : IDto
    {
        Guid Id { get; set; }
    }
}
