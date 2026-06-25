using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.SharedKernel.Events
{
    public class InMemoryEventBus : IEventBus
    {
        private readonly IServiceProvider _serviceProvider;

        public InMemoryEventBus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IEvent
        {
            var handlers = _serviceProvider.GetServices<IEventHandler<TEvent>>();

            foreach (var handler in handlers)
            {
                await handler.HandleAsync(@event);
            }
        }
    }
}
