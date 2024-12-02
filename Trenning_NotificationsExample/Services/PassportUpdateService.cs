using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;
using Trenning_NotificationsExample.Models;

namespace Trenning_NotificationsExample.Services
{
    public class PassportUpdateService
    {
        private readonly string _connectionString;
        private const int batchSize = 5000;
        private readonly string _filePath;       

        public PassportUpdateService(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<List<PassportChange>> LoadChangesAsync()
        {
            if (!File.Exists(_filePath))
                return new List<PassportChange>();

            string json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<PassportChange>>(json) ?? new List<PassportChange>();
        }

        public async Task SaveChangesAsync(List<PassportChange> changes)
        {
            string json = JsonSerializer.Serialize(changes, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, json);
        }

        public virtual async Task AddChangeAsync(PassportChange change)
        {
            var changes = await LoadChangesAsync();
            changes.Add(change);
            await SaveChangesAsync(changes);
        }
    }
}