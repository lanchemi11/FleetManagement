using FleetManagement.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Trips.Domain
{
    public class Trip : Entity
    {
        public Guid VehicleId { get; private set; }
        public Guid DriverId { get; private set; }
        public string StartLocation { get; private set; }
        public string EndLocation { get; private set; }
        public int StartMileage { get; private set; }
        public int? EndMileage { get; private set; }
        public bool IsCompleted { get; private set; }

        public Trip(Guid vehicleId, Guid driverId, string startLocation, string endLocation, int startMileage)
        {
            if (vehicleId == Guid.Empty)
                throw new ArgumentException("ID vozila mora biti validan.", nameof(vehicleId));

            if (driverId == Guid.Empty)
                throw new ArgumentException("ID vozača mora biti validan.", nameof(driverId));

            if (string.IsNullOrWhiteSpace(startLocation))
                throw new ArgumentException("Početna lokacija ne može biti prazna.", nameof(startLocation));

            if (string.IsNullOrWhiteSpace(endLocation))
                throw new ArgumentException("Krajnja lokacija ne može biti prazna.", nameof(endLocation));

            if (startMileage < 0)
                throw new ArgumentException("Početna kilometraža ne može biti negativna.", nameof(startMileage));

            VehicleId = vehicleId;
            DriverId = driverId;
            StartLocation = startLocation;
            EndLocation = endLocation;
            StartMileage = startMileage;
            IsCompleted = false;
        }

        [System.Text.Json.Serialization.JsonConstructor]
        public Trip(Guid id, Guid vehicleId, Guid driverId, string startLocation, string endLocation, int startMileage, int? endMileage, bool isCompleted)
        {
            Id = id;
            VehicleId = vehicleId;
            DriverId = driverId;
            StartLocation = startLocation;
            EndLocation = endLocation;
            StartMileage = startMileage;
            EndMileage = endMileage;
            IsCompleted = isCompleted;
        }

        public void Complete(int endMileage)
        {
            if (IsCompleted)
            {
                throw new InvalidOperationException("Ova vožnja je već završena.");
            }

            if (endMileage < StartMileage)
            {
                throw new ArgumentException($"Krajnja kilometraža ({endMileage}) ne može biti manja od početne ({StartMileage}).", nameof(endMileage));
            }

            EndMileage = endMileage;
            IsCompleted = true;
        }

        public int GetDistanceTraveled()
        {
            if (!IsCompleted || !EndMileage.HasValue)
            {
                return 0;
            }
            return EndMileage.Value - StartMileage;
        }
    }
}
