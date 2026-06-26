using FleetManagement.Modules.Vehicles.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Maintenance.Application
{
    public class ReleaseFromMaintenanceUseCase
    {
        private readonly IVehicleRepository _vehicleRepository;

        public ReleaseFromMaintenanceUseCase(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task ExecuteAsync(Guid vehicleId)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
            if (vehicle == null)
            {
                throw new InvalidOperationException("Vozilo ne postoji u sistemu.");
            }

            vehicle.ReleaseFromMaintenance();

            await _vehicleRepository.UpdateAsync(vehicle);
        }
    }
}
