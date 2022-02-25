using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AHSPersonDetection.MongoDB.Models
{
    public class AHyS
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("mothAndYear")]
        public string? MonthAndYear { get; set; }

        [BsonElement("placeName")]
        public string? PlaceName { get; set; }

        [BsonElement("monthDay")]
        public string? MonthDay { get; set; }

        [BsonElement("hour")]
        public string? Hour { get; set; }

        [BsonElement("amountOfPeople")]
        public string? AmountOfPeople { get; set; }

        [BsonElement("imageUrl")]
        public string? ImageUrl { get; set; }
    }
}
