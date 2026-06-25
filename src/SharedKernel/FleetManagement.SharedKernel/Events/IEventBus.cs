using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.SharedKernel.Events
{
    public interface IEventBus
    {
        Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IEvent;
    }
}
