using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Trenning_NotificationsExample.Models
{
    public class InactivePassports
    {
        // Устанавливаем составное поле Series_Number как первичный ключ
        [BsonId]
        [BsonRepresentation(BsonType.String)] // Используем строковое представление
        public string Id { get; set; }
               
    }
}
