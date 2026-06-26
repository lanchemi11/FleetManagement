using FleetManagement.Modules.Drivers.Application;
using FleetManagement.Modules.Drivers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Drivers.Infrastructure
{
    public class InMemoryDriverRepository : IDriverRepository
    {
        private static readonly List<Driver> _drivers = new();

        public Task<Driver?> GetByIdAsync(Guid id)
        {
            var driver = _drivers.FirstOrDefault(d => d.Id == id);
            return Task.FromResult(driver);
        }

        public Task<Driver?> GetByLicenseNumberAsync(string licenseNumber)
        {
            var driver = _drivers.FirstOrDefault(d =>
                d.LicenseNumber.Equals(licenseNumber, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(driver);
        }

        public Task<IEnumerable<Driver>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Driver>>(_drivers);
        }

        public Task AddAsync(Driver driver)
        {
            _drivers.Add(driver);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Driver driver)
        {
            var index = _drivers.FindIndex(d => d.Id == driver.Id);
            if (index != -1)
            {
                _drivers[index] = driver;
            }
            return Task.CompletedTask;
        }
    }
}
