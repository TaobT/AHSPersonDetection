using MongoDB.Driver;
using MongoDB.Bson;
using AHSPersonDetection.MongoDB.Models;

namespace AHSPersonDetection.MongoDB
{
    internal static class Database
    {
        public static IMongoDatabase? database;
        public static void Connect()
        {
            Console.WriteLine("Connecting to the Database...");
            try
            {
                var settings = MongoClientSettings.FromConnectionString("mongodb://localhost:27017");
                settings.ServerApi = new ServerApi(ServerApiVersion.V1);
                var client = new MongoClient(settings);
                database = client.GetDatabase("AHS");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot connect: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("Connected...");
            }
        }

        //public void Test()
        //{
        //    IMongoCollection<BsonDocument>? collection = database?.GetCollection<BsonDocument>("AHyS");

        //    AHyS registro = new AHyS() { MonthAndYear = "03/2022", monthRecord = new MonthRecord() { Place = "Plaza" }, placeRecords = new PlaceRecords() { MonthDay = 2 }, dayRecords = new DayRecords() { Date = "02/03/2022", Hour = "15:04", ImageUrl = "testUrl", PeopleQuantity = 10 } };

        //    collection?.InsertOne(new BsonDocument(registro.ToBSON()));
        //}

        public static List<InputData> GetUnprocessedData()
        {
            IMongoCollection<BsonDocument>? collection = database?.GetCollection<BsonDocument>("datoentradas");

            List<InputData> inputDatas = new List<InputData>();

            var results = collection.Find(x => x["Procesada"] == false).Limit(20).ToList();
            int index = 0;

            var filter = Builders<BsonDocument>.Filter.Eq("Procesada", false);
            var update = Builders<BsonDocument>.Update.Set("Procesada", true);

            foreach(BsonDocument result in results)
            {
                collection.UpdateOne(filter, update);
                try
                {
                    inputDatas.Add(new InputData() {ID_Entrada = (int) result["ID_Entrada"] ,ID_Lugar = (int) result["ID_Lugar"], Fecha = (DateTime) result["Fecha"], UrlImagen = result["UrlImagen"].ToString(), Procesada = result["Procesada"].ToBoolean() });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            if (inputDatas.Count <= 0) Console.WriteLine("No unprocessed in collection!");
            return inputDatas;
        }

        public static string GetLugar(int idLugar)
        {
            IMongoCollection<BsonDocument>? collection = database?.GetCollection<BsonDocument>("lugares");

            var results = collection.Find(x => x["ID_Lugar"] == idLugar).ToList();

            if (results.Count <= 0) return "";
            return results[0]["Nombre"].ToString();
        }
    }
}
