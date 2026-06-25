using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Vehicles.Application
{
    public record RegisterVehicleCommand(
        string Make,
        string Model,
        string PlateNumber,
        int InitialMileage
    );
}
