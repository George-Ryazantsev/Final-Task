using Microsoft.EntityFrameworkCore;


namespace Trenning_NotificationsExample.Models
{
    public class PassportContext : DbContext
    {
        public PassportContext(DbContextOptions<PassportContext> options) : base(options)
        {

        }        
        public DbSet<InactivePassport> InactivePassports { get; set; }
        public DbSet<PassportChange> PassportChanges { get; set; }
    }
}
