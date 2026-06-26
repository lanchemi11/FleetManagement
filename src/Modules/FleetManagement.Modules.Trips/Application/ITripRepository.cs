using FleetManagement.Modules.Trips.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Trips.Application
{
    public interface ITripRepository
    {
        Task<Trip?> GetByIdAsync(Guid id);
        Task<IEnumerable<Trip>> GetAllAsync();
        Task AddAsync(Trip trip);
        Task UpdateAsync(Trip trip);
    }
}
