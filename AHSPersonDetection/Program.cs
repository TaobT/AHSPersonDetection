using AHSPersonDetection.Detection;
using AHSPersonDetection.MongoDB;
using AHSPersonDetection.MongoDB.Models;
using AHSPersonDetection.Utils;

namespace AHSPersonDetection
{
    class Program
    {
        public static void Main()
        {
            Database database = new Database();
            database.Connect();
            List<InputData> unprocessedData = database.GetUnprocessedData();

            int index = 0;
            foreach(InputData input in unprocessedData)
            {
                Image.DownloadImage(input.ImageUrl, Path.Combine(@"../../../Detection/Assets", "Input/") + $"image{index++}.jpg");
            }
            

            //Console.WriteLine("Contando personas...");
            //Console.WriteLine("");
            //int persons = 0;
            //foreach (Prediction prediction in Prediction.GetPredictions("image.jpg"))
            //{
            //    if (prediction.Label == "person") persons++;
            //    Console.WriteLine("Persona Detectada. Confianza: " + prediction.Confidence);
            //    Console.WriteLine("");
            //}
            //Console.WriteLine("Personas: " + persons);
        }
    }
}