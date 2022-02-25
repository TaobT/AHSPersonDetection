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

            AHyS ahs = new() { MonthAndYear = "02/2022", Hour = "10:15", MonthDay = "25", ImageUrl = "urlTest", PlaceName = "UTNG" };
            collection?.InsertOne(ahs);
        }
    }
}
