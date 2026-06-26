using FleetManagement.Modules.Drivers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Drivers.Application
{
    public interface IDriverRepository
    {
        Task<Driver?> GetByIdAsync(Guid id);
        Task<Driver?> GetByLicenseNumberAsync(string licenseNumber);
        Task<IEnumerable<Driver>> GetAllAsync();
        Task AddAsync(Driver driver);
        Task UpdateAsync(Driver driver);
    }
}
