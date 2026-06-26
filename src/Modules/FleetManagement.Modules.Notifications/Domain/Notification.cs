using FleetManagement.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Notifications.Domain
{
    public class Notification : Entity
    {
        public string Message { get; private set; }
        public bool IsRead { get; private set; }

        public Notification(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Poruka obaveštenja ne može biti prazna.", nameof(message));

            Message = message;
            IsRead = false;
        }

        [JsonConstructor]
        public Notification(Guid id, string message, bool isRead, DateTime createdAt)
        {
            Id = id;
            Message = message;
            IsRead = isRead;
            CreatedAt = createdAt;
        }

        public void MarkAsRead()
        {
            IsRead = true;
        }
    }
}
