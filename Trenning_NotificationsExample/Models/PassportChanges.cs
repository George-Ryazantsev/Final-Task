using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Trenning_NotificationsExample.Models
{
    public class PassportChanges
    {        
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("Series")]
        public string Series { get; set; }

        [BsonElement("Number")]
        public string Number { get; set; }

        [BsonElement("ChangeType")]
        public string ChangeType { get; set; }

        [BsonElement("ChangeDate")]
        public DateTime ChangeDate { get; set; }
    }
}
