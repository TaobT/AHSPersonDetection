using MongoDB.Bson;

namespace AHSPersonDetection.MongoDB.Models
{
    public class InputData
    {
        public ObjectId Id;
        public ObjectId ID_Local;
        public string Fecha;
        public string UrlImagen;
        public bool Procesada;

    }
}
