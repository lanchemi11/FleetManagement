using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Trips.Application
{
    public record StartTripCommand(
        Guid VehicleId,
        Guid DriverId,
        string StartLocation,
        string EndLocation
    );
}
