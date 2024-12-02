using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trenning_NotificationsExample.Models;
using Trenning_NotificationsExample.Services;

namespace Trenning_NotificationsExample.Tests.Stub_classes
{
    public class FakePassportUpdateService : PassportUpdateService
    {
        public FakePassportUpdateService(string filePath) : base(filePath)
        {
        }

        public List<PassportChange> Changes { get; } = new List<PassportChange>();

        public override async Task AddChangeAsync(PassportChange change)
        {
            Changes.Add(change);
            await Task.CompletedTask;
        }
    }
}
