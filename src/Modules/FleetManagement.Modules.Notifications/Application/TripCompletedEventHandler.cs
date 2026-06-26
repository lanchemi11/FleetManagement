using FleetManagement.Modules.Notifications.Domain;
using FleetManagement.SharedKernel.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Notifications.Application
{
    public class TripCompletedEventHandler : IEventHandler<TripCompletedEvent>
    {
        private readonly INotificationRepository _notificationRepository;

        public TripCompletedEventHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task HandleAsync(TripCompletedEvent @event)
        {
            string message = $"Vožnja sa vozilom {@event.VehiclePlateNumber} koju je vozio {@event.DriverFullName} je uspešno završena. Pređeno je {@event.DistanceTraveled} km.";

            var notification = new Notification(message);

            await _notificationRepository.AddAsync(notification);
        }
    }
}
