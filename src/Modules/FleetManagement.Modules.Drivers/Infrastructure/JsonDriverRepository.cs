using FleetManagement.Modules.Drivers.Application;
using FleetManagement.Modules.Drivers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Drivers.Infrastructure
{
    public class JsonDriverRepository : IDriverRepository
    {
        private readonly string _filePath = "drivers.json";
        private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

        private async Task<List<Driver>> ReadFromFileAsync()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Driver>();
            }

            var json = await File.ReadAllTextAsync(_filePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<Driver>();
            }

            try
            {
                return JsonSerializer.Deserialize<List<Driver>>(json, _options) ?? new List<Driver>();
            }
            catch
            {
                return new List<Driver>();
            }
        }

        private async Task WriteToFileAsync(List<Driver> drivers)
        {
            var json = JsonSerializer.Serialize(drivers, _options);
            await File.WriteAllTextAsync(_filePath, json);
        }

        public async Task<Driver?> GetByIdAsync(Guid id)
        {
            var drivers = await ReadFromFileAsync();
            return drivers.FirstOrDefault(d => d.Id == id);
        }

        public async Task<Driver?> GetByLicenseNumberAsync(string licenseNumber)
        {
            var drivers = await ReadFromFileAsync();
            return drivers.FirstOrDefault(d =>
                d.LicenseNumber.Equals(licenseNumber, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<IEnumerable<Driver>> GetAllAsync()
        {
            return await ReadFromFileAsync();
        }

        public async Task AddAsync(Driver driver)
        {
            var drivers = await ReadFromFileAsync();
            drivers.Add(driver);
            await WriteToFileAsync(drivers);
        }

        public async Task UpdateAsync(Driver driver)
        {
            var drivers = await ReadFromFileAsync();
            var index = drivers.FindIndex(d => d.Id == driver.Id);
            if (index != -1)
            {
                drivers[index] = driver;
                await WriteToFileAsync(drivers);
            }
        }
    }
}
