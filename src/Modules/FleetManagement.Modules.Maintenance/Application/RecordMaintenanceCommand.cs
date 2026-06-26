using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Maintenance.Application
{
    public record RecordMaintenanceCommand(
        Guid VehicleId,
        string Description,
        decimal Cost
    );
}
