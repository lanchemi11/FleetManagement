using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Drivers.Application
{
    public record RegisterDriverCommand(
        string FirstName,
        string LastName,
        string LicenseNumber
    );
}
