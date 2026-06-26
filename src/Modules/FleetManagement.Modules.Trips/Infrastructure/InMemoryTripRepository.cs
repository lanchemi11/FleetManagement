using FleetManagement.Modules.Trips.Application;
using FleetManagement.Modules.Trips.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Trips.Infrastructure
{
    public class InMemoryTripRepository : ITripRepository
    {
        private static readonly List<Trip> _trips = new();

        public Task<Trip?> GetByIdAsync(Guid id)
        {
            var trip = _trips.FirstOrDefault(t => t.Id == id);
            return Task.FromResult(trip);
        }

        public Task<IEnumerable<Trip>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Trip>>(_trips);
        }

        public Task AddAsync(Trip trip)
        {
            _trips.Add(trip);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Trip trip)
        {
            var index = _trips.FindIndex(t => t.Id == trip.Id);
            if (index != -1)
            {
                _trips[index] = trip;
            }
            return Task.CompletedTask;
        }
    }
}
