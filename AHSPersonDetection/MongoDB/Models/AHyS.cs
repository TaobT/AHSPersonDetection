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
        [BsonElement("monthRecords")]
        public MonthRecord? monthRecord { get; set; }
    }

    public class MonthRecord
    {
        [BsonElement("place")]
        public string? Place { get; set; }
        [BsonElement("placeRecords")]
        public PlaceRecords? placeRecords { get; set; }
    }

    public class PlaceRecords
    {
        [BsonElement("monthDay")]
        public int? MonthDay { get; set; }
        [BsonElement("dayRecords")]
        public DayRecords? dayRecords { get; set; }
    }

    public class DayRecords
    {
        [BsonElement("hour")]
        public string? Hour { get; set; }
        [BsonElement("date")]
        public string? Date { get; set; }
        [BsonElement("peopleQuantity")]
        public int? PeopleQuantity { get; set; }
        [BsonElement("imageUrl")]
        public string? ImageUrl { get; set; }
    }
}
