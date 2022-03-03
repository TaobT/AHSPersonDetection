using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AHSPersonDetection.MongoDB.Models
{
    public class AHyS
    {
        [BsonElement("mothAndYear")]
        public string? MonthAndYear { get; set; }
        public MonthRecord? monthRecord { get; set; }
        public PlaceRecords? placeRecords { get; set; }
        public DayRecords? dayRecords { get; set; }

        public BsonDocument ToBSON()
        {
            BsonDocument bsonDocument = new BsonDocument()
            {
                {"monthAndYear", MonthAndYear },
                {"monthRecords", new BsonArray
                {
                    new BsonDocument { { "place", monthRecord?.Place }, {"placeRecords", new BsonArray { 
                        new BsonDocument { { "monthDay", placeRecords?.MonthDay }, { "dayRecords", new BsonArray {
                        new BsonDocument { { "hour", dayRecords?.Hour }, { "date", dayRecords?.Date }, { "peopleQuantity", dayRecords?.PeopleQuantity }, { "imageUrl", dayRecords?.ImageUrl } } } } }
                    } } }
                } }
            };

            return bsonDocument;
        }
    }

    public class MonthRecord
    {
        public string? Place { get; set; }
    }

    public class PlaceRecords
    {
        public int? MonthDay { get; set; }
    }

    public class DayRecords
    {
        public string? Hour { get; set; }
        public string? Date { get; set; }
        public int? PeopleQuantity { get; set; }
        public string? ImageUrl { get; set; }
    }
}
