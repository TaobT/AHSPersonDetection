using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AHSPersonDetection.MongoDB.Models
{
    public class AHyS
    {
        public int ID_Entrada;
        public int ID_Lugar;
        public string Nombre_Lugar;
        public DateTime Fecha;
        public int CantidadPersonas;
        public string UrlImagen;
    }

}
