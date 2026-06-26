using FleetManagement.Modules.Drivers.Application;
using FleetManagement.Modules.Vehicles.Application;
using FleetManagement.SharedKernel.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Trips.Application
{
    public class CompleteTripUseCase
    {
        private readonly ITripRepository _tripRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IDriverRepository _driverRepository;
        private readonly IEventBus _eventBus;

        public CompleteTripUseCase(
            ITripRepository tripRepository,
            IVehicleRepository vehicleRepository,
            IDriverRepository driverRepository,
            IEventBus eventBus)
        {
            _tripRepository = tripRepository;
            _vehicleRepository = vehicleRepository;
            _driverRepository = driverRepository;
            _eventBus = eventBus;
        }

        public async Task ExecuteAsync(CompleteTripCommand command)
        {
            var trip = await _tripRepository.GetByIdAsync(command.TripId);
            if (trip == null)
            {
                throw new InvalidOperationException("Tražena vožnja ne postoji u sistemu.");
            }

            if (trip.IsCompleted)
            {
                throw new InvalidOperationException("Ova vožnja je već završena.");
            }

            var vehicle = await _vehicleRepository.GetByIdAsync(trip.VehicleId);
            var driver = await _driverRepository.GetByIdAsync(trip.DriverId);

            if (vehicle == null || driver == null)
            {
                throw new InvalidOperationException("Podaci o vozilu ili vozaču koji su povezani sa ovom vožnjom više ne postoje.");
            }

            trip.Complete(command.EndMileage);

            vehicle.CompleteTrip(command.EndMileage);

            driver.Release();

            await _tripRepository.UpdateAsync(trip);
            await _vehicleRepository.UpdateAsync(vehicle);
            await _driverRepository.UpdateAsync(driver);

            int distanceTraveled = trip.GetDistanceTraveled();
            string driverFullName = $"{driver.FirstName} {driver.LastName}";

            var tripCompletedEvent = new TripCompletedEvent(
                trip.Id,
                vehicle.PlateNumber,
                driverFullName,
                distanceTraveled
            );

            await _eventBus.PublishAsync(tripCompletedEvent);
        }
    }
}
