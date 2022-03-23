using AHSPersonDetection.Detection;
using AHSPersonDetection.MongoDB;
using AHSPersonDetection.MongoDB.Models;
using AltoHttp;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AHSPersonDetection
{
    public static class AHSProcess
    {
        private static List<AHyS> UnprocessedData = new List<AHyS>();
        private static List<AHyS> ProcessedData = new List<AHyS>();
        private static List<InputData> NewData = new List<InputData>();
        private static DownloadQueue downloadQueue = new DownloadQueue();
        public static void Initialize()
        {
            Database.Connect();
        }

        public static void AddUnprocessedData()
        {
            Console.WriteLine("Getting UnprocessedData...");
            if (NewData.Count >= Constants.MAX_UNPROCESSED_DATA) return;
            NewData.AddRange(Database.GetUnprocessedData());
        }

        public static void DownloadImages()
        {
            Console.WriteLine("Downloading Images...");
            if (NewData.Count >= 20) return;
            int index = 0;
            foreach (InputData data in NewData)
            {
                downloadQueue.Add(data.UrlImagen, Path.Combine(@"../../../Detection/Assets", "Input/") + $"image{index++}.jpg");
                UnprocessedData.Add(new AHyS
                {
                    ID_DatosEntrada = (ObjectId) data.Id,
                    ID_Local = (ObjectId) data.ID_Local,
                    Fecha = data.Fecha,
                    DirImagen = Path.Combine(@"../../../Detection/Assets", "Input/") + $"image{index++}.jpg"
                });
            }
            downloadQueue.StartAsync();
            NewData.Clear();
        }

        public static void ScanImages()
        {
            Console.WriteLine("Scanning Images...");
            string[] images = Directory.GetFiles(Path.Combine(@"../../../Detection/Assets", "Input/"));
            if (UnprocessedData.Count <= 0) return;
            foreach (string image in images)
            {
                AHyS uData = UnprocessedData.Find(x => x.DirImagen.Equals(image, StringComparison.Ordinal));
                int people = 0;
                foreach (Prediction prediction in Prediction.GetPredictions(image))
                {
                    if (prediction.Label == "person") people++;
                }
                ProcessedData.Add(new AHyS
                {
                    ID_DatosEntrada = (ObjectId)uData.ID_DatosEntrada,
                    ID_Local = (ObjectId)uData.ID_Local,
                    Nombre_Lugar = uData.Nombre_Lugar,
                    Fecha = uData.Fecha,
                    CantidadPersonas = people
                });
            }
            UnprocessedData.Clear();
        }

        public static void SendProcessedDataToDatabase()
        {
            Console.WriteLine("Generating ProcessedData report (AHyS)...");
            if (ProcessedData.Count <= 0) return;
            IMongoCollection<BsonDocument>? collection = Database.database.GetCollection<BsonDocument>("AHyS");
            foreach (AHyS processedData in ProcessedData)
            {
                collection.InsertOne(new BsonDocument
                {
                    { "ID_DatosEntrada", processedData.ID_DatosEntrada },
                    { "ID_Local", processedData.ID_Local  },
                    { "Nombre_lugar", processedData.Nombre_Lugar },
                    { "CantidadPersonas", processedData.CantidadPersonas }
                });
            }
            ProcessedData.Clear();
        }
    }
}
