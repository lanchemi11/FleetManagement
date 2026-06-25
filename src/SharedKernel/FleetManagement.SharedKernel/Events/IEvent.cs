using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.SharedKernel.Events
{
    public interface IEvent
    {
        DateTime OccurredOn { get; }
    }
}
