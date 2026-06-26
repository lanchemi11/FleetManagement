using FleetManagement.Modules.Maintenance.Application;
using FleetManagement.Modules.Maintenance.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FleetManagement.Modules.Maintenance.Infrastructure
{
    public class JsonMaintenanceRepository : IMaintenanceRepository
    {
        private readonly string _filePath = "maintenance.json";
        private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

        private async Task<List<MaintenanceRecord>> ReadFromFileAsync()
        {
            if (!File.Exists(_filePath))
            {
                return new List<MaintenanceRecord>();
            }

            var json = await File.ReadAllTextAsync(_filePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<MaintenanceRecord>();
            }

            try
            {
                return JsonSerializer.Deserialize<List<MaintenanceRecord>>(json, _options) ?? new List<MaintenanceRecord>();
            }
            catch
            {
                return new List<MaintenanceRecord>();
            }
        }

        private async Task WriteToFileAsync(List<MaintenanceRecord> records)
        {
            var json = JsonSerializer.Serialize(records, _options);
            await File.WriteAllTextAsync(_filePath, json);
        }

        public async Task<MaintenanceRecord?> GetByIdAsync(Guid id)
        {
            var records = await ReadFromFileAsync();
            return records.FirstOrDefault(r => r.Id == id);
        }

        public async Task<IEnumerable<MaintenanceRecord>> GetAllAsync()
        {
            return await ReadFromFileAsync();
        }

        public async Task AddAsync(MaintenanceRecord record)
        {
            var records = await ReadFromFileAsync();
            records.Add(record);
            await WriteToFileAsync(records);
        }

        public async Task UpdateAsync(MaintenanceRecord record)
        {
            var records = await ReadFromFileAsync();
            var index = records.FindIndex(r => r.Id == record.Id);
            if (index != -1)
            {
                records[index] = record;
                await WriteToFileAsync(records);
            }
        }
    }
}
