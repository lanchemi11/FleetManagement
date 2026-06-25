using FleetManagement.Modules.Vehicles.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Vehicles.Application
{
    public class RegisterVehicleUseCase
    {
        private readonly IVehicleRepository _vehicleRepository;

        public RegisterVehicleUseCase(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task ExecuteAsync(RegisterVehicleCommand command)
        {
            var existingVehicle = await _vehicleRepository.GetByPlateNumberAsync(command.PlateNumber);
            if (existingVehicle != null)
            {
                throw new InvalidOperationException($"Vozilo sa registarskom oznakom '{command.PlateNumber}' već postoji u bazi podataka.");
            }

            var vehicle = new Vehicle(
                command.Make,
                command.Model,
                command.PlateNumber,
                command.InitialMileage
            );

            await _vehicleRepository.AddAsync(vehicle);
        }
    }
}
