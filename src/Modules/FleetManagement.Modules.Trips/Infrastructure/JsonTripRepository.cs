using FleetManagement.Modules.Trips.Application;
using FleetManagement.Modules.Trips.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Trips.Infrastructure
{
    public class JsonTripRepository : ITripRepository
    {
        private readonly string _filePath = "trips.json";
        private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

        private async Task<List<Trip>> ReadFromFileAsync()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Trip>();
            }

            var json = await File.ReadAllTextAsync(_filePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<Trip>();
            }

            try
            {
                return JsonSerializer.Deserialize<List<Trip>>(json, _options) ?? new List<Trip>();
            }
            catch
            {
                return new List<Trip>();
            }
        }

        private async Task WriteToFileAsync(List<Trip> trips)
        {
            var json = JsonSerializer.Serialize(trips, _options);
            await File.WriteAllTextAsync(_filePath, json);
        }

        public async Task<Trip?> GetByIdAsync(Guid id)
        {
            var trips = await ReadFromFileAsync();
            return trips.FirstOrDefault(t => t.Id == id);
        }

        public async Task<IEnumerable<Trip>> GetAllAsync()
        {
            return await ReadFromFileAsync();
        }

        public async Task AddAsync(Trip trip)
        {
            var trips = await ReadFromFileAsync();
            trips.Add(trip);
            await WriteToFileAsync(trips);
        }

        public async Task UpdateAsync(Trip trip)
        {
            var trips = await ReadFromFileAsync();
            var index = trips.FindIndex(t => t.Id == trip.Id);
            if (index != -1)
            {
                trips[index] = trip;
                await WriteToFileAsync(trips);
            }
        }
    }
}
