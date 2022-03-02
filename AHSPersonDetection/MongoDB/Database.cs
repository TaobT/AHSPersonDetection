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
            IMongoCollection<AHyS>? collection = database?.GetCollection<AHyS>("AHyS");

            AHyS ahs = new() { MonthAndYear = "03/2022", monthRecord = new MonthRecord() { Place = "Plaza", placeRecords = new PlaceRecords() { MonthDay = 1, dayRecords = new DayRecords() { Hour = "23:35", Date = "01/03/2022" , PeopleQuantity = 10, ImageUrl = "exampleImageUrle" } } } };
            collection?.InsertOne(ahs);
        }
    }
}
