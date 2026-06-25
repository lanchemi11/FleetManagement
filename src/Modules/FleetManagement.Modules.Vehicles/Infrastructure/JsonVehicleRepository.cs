using FleetManagement.Modules.Vehicles.Application;
using FleetManagement.Modules.Vehicles.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Vehicles.Infrastructure
{
    public class JsonVehicleRepository : IVehicleRepository
    {
        private readonly string _filePath = "vehicles.json";
        private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

        private async Task<List<Vehicle>> ReadFromFileAsync()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Vehicle>();
            }

            var json = await File.ReadAllTextAsync(_filePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<Vehicle>();
            }

            try
            {
                return JsonSerializer.Deserialize<List<Vehicle>>(json, _options) ?? new List<Vehicle>();
            }
            catch
            {
                return new List<Vehicle>();
            }
        }

        private async Task WriteToFileAsync(List<Vehicle> vehicles)
        {
            var json = JsonSerializer.Serialize(vehicles, _options);
            await File.WriteAllTextAsync(_filePath, json);
        }

        public async Task<Vehicle?> GetByIdAsync(Guid id)
        {
            var vehicles = await ReadFromFileAsync();
            return vehicles.FirstOrDefault(v => v.Id == id);
        }

        public async Task<Vehicle?> GetByPlateNumberAsync(string plateNumber)
        {
            var vehicles = await ReadFromFileAsync();
            return vehicles.FirstOrDefault(v =>
                v.PlateNumber.Equals(plateNumber, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await ReadFromFileAsync();
        }

        public async Task AddAsync(Vehicle vehicle)
        {
            var vehicles = await ReadFromFileAsync();
            vehicles.Add(vehicle);
            await WriteToFileAsync(vehicles);
        }

        public async Task UpdateAsync(Vehicle vehicle)
        {
            var vehicles = await ReadFromFileAsync();
            var index = vehicles.FindIndex(v => v.Id == vehicle.Id);
            if (index != -1)
            {
                vehicles[index] = vehicle;
                await WriteToFileAsync(vehicles);
            }
        }
    }
}
