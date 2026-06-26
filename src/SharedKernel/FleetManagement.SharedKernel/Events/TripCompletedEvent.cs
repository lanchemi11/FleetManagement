using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.SharedKernel.Events
{
    public class TripCompletedEvent : IEvent
    {
        public Guid TripId { get; }
        public string VehiclePlateNumber { get; }
        public string DriverFullName { get; }
        public int DistanceTraveled { get; }
        public DateTime OccurredOn { get; }

        public TripCompletedEvent(Guid tripId, string vehiclePlateNumber, string driverFullName, int distanceTraveled)
        {
            TripId = tripId;
            VehiclePlateNumber = vehiclePlateNumber;
            DriverFullName = driverFullName;
            DistanceTraveled = distanceTraveled;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
