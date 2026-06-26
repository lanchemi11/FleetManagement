using FleetManagement.Modules.Notifications.Application;
using FleetManagement.Modules.Notifications.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Notifications.Infrastructure
{
    public class JsonNotificationRepository : INotificationRepository
    {
        private readonly string _filePath = "notifications.json";
        private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

        private async Task<List<Notification>> ReadFromFileAsync()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Notification>();
            }

            var json = await File.ReadAllTextAsync(_filePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<Notification>();
            }

            try
            {
                return JsonSerializer.Deserialize<List<Notification>>(json, _options) ?? new List<Notification>();
            }
            catch
            {
                return new List<Notification>();
            }
        }

        private async Task WriteToFileAsync(List<Notification> notifications)
        {
            var json = JsonSerializer.Serialize(notifications, _options);
            await File.WriteAllTextAsync(_filePath, json);
        }

        public async Task<Notification?> GetByIdAsync(Guid id)
        {
            var notifications = await ReadFromFileAsync();
            return notifications.FirstOrDefault(n => n.Id == id);
        }

        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            return await ReadFromFileAsync();
        }

        public async Task AddAsync(Notification notification)
        {
            var notifications = await ReadFromFileAsync();
            notifications.Add(notification);
            await WriteToFileAsync(notifications);
        }

        public async Task UpdateAsync(Notification notification)
        {
            var notifications = await ReadFromFileAsync();
            var index = notifications.FindIndex(n => n.Id == notification.Id);
            if (index != -1)
            {
                notifications[index] = notification;
                await WriteToFileAsync(notifications);
            }
        }
    }
}
