using FleetManagement.Modules.Drivers.Application;
using FleetManagement.Modules.Trips.Domain;
using FleetManagement.Modules.Vehicles.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Trips.Application
{
    public class StartTripUseCase
    {
        private readonly ITripRepository _tripRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IDriverRepository _driverRepository;

        public StartTripUseCase(
            ITripRepository tripRepository,
            IVehicleRepository vehicleRepository,
            IDriverRepository driverRepository)
        {
            _tripRepository = tripRepository;
            _vehicleRepository = vehicleRepository;
            _driverRepository = driverRepository;
        }

        public async Task ExecuteAsync(StartTripCommand command)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(command.VehicleId);
            if (vehicle == null)
            {
                throw new InvalidOperationException("Izabrano vozilo ne postoji u sistemu.");
            }

            var driver = await _driverRepository.GetByIdAsync(command.DriverId);
            if (driver == null)
            {
                throw new InvalidOperationException("Izabrani vozač ne postoji u sistemu.");
            }

            vehicle.AssignToTrip();
            driver.AssignToTrip();

            var trip = new Trip(
                command.VehicleId,
                command.DriverId,
                command.StartLocation,
                command.EndLocation,
                vehicle.CurrentMileage
            );

            await _vehicleRepository.UpdateAsync(vehicle);
            await _driverRepository.UpdateAsync(driver);
            await _tripRepository.AddAsync(trip);
        }
    }
}
