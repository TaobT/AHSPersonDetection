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
        private static List<AHySP> UnprocessedData = new List<AHySP>();
        private static List<AHyS> ProcessedData = new List<AHyS>();
        private static List<InputData> NewData = new List<InputData>();
        private static DownloadQueue downloadQueue = new DownloadQueue();
        public static void Initialize()
        {
            Database.Connect();
        }

        public static async void AddNewData()
        {
            await Task.Run(() =>
            {
                if (NewData.Count >= Constants.MAX_UNPROCESSED_DATA) return;
                Console.WriteLine("Getting UnprocessedData...");
                NewData.AddRange(Database.GetUnprocessedData());
            });
        }

        public static async void DownloadImages()
        {
            await Task.Run(() =>
            {
                if (NewData.Count >= 20) return;
            Console.WriteLine("Downloading Images...");
            int index = 0;
            foreach (InputData data in NewData.ToList())
            {
                downloadQueue.Add(data.UrlImagen, Path.Combine(@"../../../Detection/Assets", "Input/") + $"image{index++}.jpg");
                UnprocessedData.Add(new AHySP
                {
                    ID_Entrada = data.ID_Entrada,
                    ID_Lugar = data.ID_Lugar,
                    Fecha = data.Fecha,
                    UrlImagen = data.UrlImagen,
                    DirImagen = Path.Combine(@"../../../Detection/Assets", "Input/") + $"image{index}.jpg"
                });
            }
            downloadQueue.StartAsync();
            NewData.Clear();
            });
        }

        public static async void ScanImages()
        {
            await Task.Run(() =>
            {
                if (UnprocessedData.Count <= 0) return;
                Console.WriteLine("Scanning Images...");
                string[] images = Directory.GetFiles(Path.Combine(@"../../../Detection/Assets", "Input/"));
                foreach (string image in images)
                {
                    AHySP uData = UnprocessedData.Find(x => x.DirImagen.Equals(image, StringComparison.Ordinal));
                    int people = 0;
                    foreach (Prediction prediction in Prediction.GetPredictions(image))
                    {
                        if (prediction.Label == "person") people++;
                    }
                    if (uData != null)
                    {
                        string Nombre_Lugar = Database.GetLugar(uData.ID_Lugar);
                        ProcessedData.Add(new AHyS
                        {
                            ID_Entrada = uData.ID_Entrada,
                            ID_Lugar = uData.ID_Lugar,
                            Nombre_Lugar = Nombre_Lugar,
                            Fecha = uData.Fecha,
                            UrlImagen = uData.UrlImagen,
                            CantidadPersonas = people
                        });
                    }
                }
                UnprocessedData.Clear();
            });
        }

        public static async void SendProcessedDataToDatabase()
        {
            await Task.Run(() =>
            {
                if (ProcessedData.Count <= 0) return;
                Console.WriteLine("Generating ProcessedData report (AHyS)...");
                IMongoCollection<BsonDocument>? collection = Database.database?.GetCollection<BsonDocument>("ahs");
                foreach (AHyS processedData in ProcessedData)
                {
                    AHyS ahs = new AHyS
                    {
                        ID_Entrada = processedData.ID_Entrada,
                        ID_Lugar = processedData.ID_Lugar,
                        Nombre_Lugar = processedData.Nombre_Lugar,
                        Fecha = processedData.Fecha,
                        UrlImagen = processedData.UrlImagen,
                        CantidadPersonas = processedData.CantidadPersonas
                    };
                    collection?.InsertOne(ahs.ToBsonDocument());
                    //collection?.InsertOne(new BsonDocument
                    //{
                    //    { "ID_Entrada", processedData.ID_Entrada },
                    //    { "ID_Lugar", processedData.ID_Lugar  },
                    //    { "Nombre_lugar", processedData.Nombre_Lugar },
                    //    { "CantidadPersonas", processedData.CantidadPersonas }
                    //});
                }
                ProcessedData.Clear();
            });
        }
    }
}
