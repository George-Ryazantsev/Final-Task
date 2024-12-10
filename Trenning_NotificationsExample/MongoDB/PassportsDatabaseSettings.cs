namespace Trenning_NotificationsExample.MongoDB
{
    public class PassportsDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string PassportsCollectionName { get; set; } = null!;
    }
}
