using FleetManagement.Modules.Maintenance.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Maintenance.Application
{
    public interface IMaintenanceRepository
    {
        Task<MaintenanceRecord?> GetByIdAsync(Guid id);
        Task<IEnumerable<MaintenanceRecord>> GetAllAsync();
        Task AddAsync(MaintenanceRecord record);
        Task UpdateAsync(MaintenanceRecord record);
    }
}
