using MongoDB.Driver;
using MongoDB.Bson;
using AHSPersonDetection.MongoDB.Models;

namespace AHSPersonDetection.MongoDB
{
    internal class Database
    {
        public IMongoDatabase? database;
        public void Connect()
        {
            var settings = MongoClientSettings.FromConnectionString("mongodb+srv://ahsBDapp:4glomeraci0nHS@cluster0.scapg.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            database = client.GetDatabase("AHSDatabase");
        }

        public void Test()
        {
            IMongoCollection<BsonDocument>? collection = database?.GetCollection<BsonDocument>("test");

            AHyS registro = new AHyS() { MonthAndYear = "03/2022", monthRecord = new MonthRecord() { Place = "Plaza" }, placeRecords = new PlaceRecords() { MonthDay = 2 }, dayRecords = new DayRecords() { Date = "02/03/2022", Hour = "15:04", ImageUrl = "testUrl", PeopleQuantity = 10 } };

            collection?.InsertOne(new BsonDocument(registro.ToBSON()));
        }
    }
}
