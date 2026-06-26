using FleetManagement.Modules.Maintenance.Domain;
using FleetManagement.Modules.Vehicles.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Maintenance.Application
{
    public class RecordMaintenanceUseCase
    {
        private readonly IMaintenanceRepository _maintenanceRepository;
        private readonly IVehicleRepository _vehicleRepository;

        public RecordMaintenanceUseCase(IMaintenanceRepository maintenanceRepository, IVehicleRepository vehicleRepository)
        {
            _maintenanceRepository = maintenanceRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task ExecuteAsync(RecordMaintenanceCommand command)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(command.VehicleId);
            if (vehicle == null)
            {
                throw new InvalidOperationException("Izabrano vozilo ne postoji u sistemu.");
            }

            vehicle.SendToMaintenance();

            var record = new MaintenanceRecord(command.VehicleId, command.Description, command.Cost);

            await _vehicleRepository.UpdateAsync(vehicle);
            await _maintenanceRepository.AddAsync(record);
        }
    }
}
