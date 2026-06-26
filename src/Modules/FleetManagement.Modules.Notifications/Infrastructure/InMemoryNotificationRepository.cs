using FleetManagement.Modules.Notifications.Application;
using FleetManagement.Modules.Notifications.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Notifications.Infrastructure
{
    public class InMemoryNotificationRepository : INotificationRepository
    {
        private static readonly List<Notification> _notifications = new();

        public Task<Notification?> GetByIdAsync(Guid id)
        {
            var notification = _notifications.FirstOrDefault(n => n.Id == id);
            return Task.FromResult(notification);
        }

        public Task<IEnumerable<Notification>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Notification>>(_notifications);
        }

        public Task AddAsync(Notification notification)
        {
            _notifications.Add(notification);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Notification notification)
        {
            var index = _notifications.FindIndex(n => n.Id == notification.Id);
            if (index != -1)
            {
                _notifications[index] = notification;
            }
            return Task.CompletedTask;
        }
    }
}
