using FleetManagement.Modules.Maintenance.Application;
using FleetManagement.Modules.Maintenance.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Maintenance.Infrastructure
{
    public class InMemoryMaintenanceRepository : IMaintenanceRepository
    {
        private static readonly List<MaintenanceRecord> _records = new();

        public Task<MaintenanceRecord?> GetByIdAsync(Guid id)
        {
            var record = _records.FirstOrDefault(r => r.Id == id);
            return Task.FromResult(record);
        }

        public Task<IEnumerable<MaintenanceRecord>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<MaintenanceRecord>>(_records);
        }

        public Task AddAsync(MaintenanceRecord record)
        {
            _records.Add(record);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(MaintenanceRecord record)
        {
            var index = _records.FindIndex(r => r.Id == record.Id);
            if (index != -1)
            {
                _records[index] = record;
            }
            return Task.CompletedTask;
        }
    }

}
