using AltoHttp;

namespace AHSPersonDetection.Utils
{
    public class Image
    {
        private static HttpDownloader httpDownloader;

        public static void DownloadImage(string url, string path)
        {
            httpDownloader = new HttpDownloader(url, path);
            httpDownloader.Start();
        }
    }
}
