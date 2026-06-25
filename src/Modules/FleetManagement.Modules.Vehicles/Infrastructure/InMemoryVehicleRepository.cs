using FleetManagement.Modules.Vehicles.Application;
using FleetManagement.Modules.Vehicles.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Vehicles.Infrastructure
{
    public class InMemoryVehicleRepository : IVehicleRepository
    {
        private static readonly List<Vehicle> _vehicles = new();

        public Task<Vehicle?> GetByIdAsync(Guid id)
        {
            var vehicle = _vehicles.FirstOrDefault(v => v.Id == id);
            return Task.FromResult(vehicle);
        }

        public Task<Vehicle?> GetByPlateNumberAsync(string plateNumber)
        {
            var vehicle = _vehicles.FirstOrDefault(v =>
                v.PlateNumber.Equals(plateNumber, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(vehicle);
        }

        public Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Vehicle>>(_vehicles);
        }

        public Task AddAsync(Vehicle vehicle)
        {
            _vehicles.Add(vehicle);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Vehicle vehicle)
        {
            var index = _vehicles.FindIndex(v => v.Id == vehicle.Id);
            if (index != -1)
            {
                _vehicles[index] = vehicle;
            }
            return Task.CompletedTask;
        }
    }
}
