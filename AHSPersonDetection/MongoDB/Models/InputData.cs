using MongoDB.Bson;

namespace AHSPersonDetection.MongoDB.Models
{
    public class InputData
    {
        public int ID_Entrada;
        public int ID_Lugar;
        public DateTime Fecha;
        public string UrlImagen;
        public bool Procesada;

    }
}
