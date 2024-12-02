namespace Trenning_NotificationsExample.Models
{
    public class PassportChange
    {
        public int Id { get; set; }
        public string Series { get; set; }
        public string Number { get; set; }
        public string ChangeType { get; set; } // Added  или Removed 
        public DateTime ChangeDate { get; set; }
    }
}
