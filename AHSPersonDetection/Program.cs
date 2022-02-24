namespace AHSPersonDetection
{
    class Program
    {
        public static void Main()
        {
            Console.WriteLine("Contando personas...");
            Console.WriteLine("");
            int persons = 0;
            foreach (Prediction prediction in Prediction.GetPredictions("D:/Code Projects/C#/AHSPersonDetection/AHSPersonDetection/Assets/Input/image.jpg"))
            {
                if (prediction.Label == "person") persons++;
                Console.WriteLine("Persona Detectada. Confianza: " + prediction.Confidence);
            }
            Console.WriteLine("");
            Console.WriteLine("Personas: " + persons);
        }
    }
}