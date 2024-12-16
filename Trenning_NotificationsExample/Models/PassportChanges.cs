using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Trenning_NotificationsExample.Models
{
    public class PassportChanges
    {
        // Устанавливаем составное поле Series_Number как первичный ключ
        [BsonId]
        [BsonRepresentation(BsonType.String)] // Используем строковое представление
        public string Id { get; set; }

        [BsonIgnore] // Убираем эти поля из сериализации, чтобы хранить только Id
        public string Series => Id.Split('_')[0]; // Автоматически извлекаем Series из Id

        [BsonIgnore]
        public string Number => Id.Split('_')[1]; // Автоматически извлекаем Number из Id

        [BsonElement("ChangeType")]
        public string ChangeType { get; set; }

        [BsonElement("ChangeDate")]
        public DateTime ChangeDate { get; set; }
    }
}
