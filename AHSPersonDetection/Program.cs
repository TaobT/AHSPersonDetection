namespace AHSPersonDetection
{
    class Program
    {
        private static bool isRunning = false;
        public static void Main()
        {
            Console.Title = "AHySDetection";
            isRunning = true;

            AHSProcess.Initialize();

            Thread mainThread = new Thread(new ThreadStart(MainThread));
            mainThread.Start();
        }

        private static void MainThread()
        {
            Console.WriteLine($"Main thread started. Running at 1 tick every 30 seconds.");
            DateTime _nextLoop = DateTime.Now;


            while (isRunning)
            {

                AHSProcess.AddNewData();
                AHSProcess.DownloadImages();
                AHSProcess.ScanImages();
                AHSProcess.SendProcessedDataToDatabase();

                _nextLoop = _nextLoop.AddSeconds(Constants.MS_PER_TICK);

                if(_nextLoop > DateTime.Now)
                {
                    Thread.Sleep(_nextLoop - DateTime.Now);
                }
            }
        }
    }
}