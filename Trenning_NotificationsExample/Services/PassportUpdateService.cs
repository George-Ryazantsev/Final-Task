﻿using System.Text.Json;
using Trenning_NotificationsExample.Models;

namespace Trenning_NotificationsExample.Services
{
    public class PassportUpdateService
    {                
        private string _filePath;                
        public string FilePath
        {            
            set => _filePath = value; 
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