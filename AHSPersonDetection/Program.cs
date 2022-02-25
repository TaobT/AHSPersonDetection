using AHSPersonDetection.Detection;
using AHSPersonDetection.MongoDB;

namespace AHSPersonDetection
{
    class Program
    {
        public static void Main()
        {
            Database database = new Database();
            database.Connect();
            database.Test();
            

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