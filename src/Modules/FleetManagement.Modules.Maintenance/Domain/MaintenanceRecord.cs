using FleetManagement.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Maintenance.Domain
{
    public class MaintenanceRecord : Entity
    {
        public Guid VehicleId { get; private set; }
        public string Description { get; private set; }
        public decimal Cost { get; private set; }
        public DateTime ServiceDate { get; private set; }

        public MaintenanceRecord(Guid vehicleId, string description, decimal cost)
        {
            if (vehicleId == Guid.Empty)
                throw new ArgumentException("ID vozila mora biti validan.", nameof(vehicleId));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Opis servisa ne može biti prazan.", nameof(description));

            if (cost < 0)
                throw new ArgumentException("Cena servisa ne može biti negativna.", nameof(cost));

            VehicleId = vehicleId;
            Description = description;
            Cost = cost;
            ServiceDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        public MaintenanceRecord(Guid id, Guid vehicleId, string description, decimal cost, DateTime serviceDate, DateTime createdAt)
        {
            Id = id;
            VehicleId = vehicleId;
            Description = description;
            Cost = cost;
            ServiceDate = serviceDate;
            CreatedAt = createdAt;
        }
    }
}
