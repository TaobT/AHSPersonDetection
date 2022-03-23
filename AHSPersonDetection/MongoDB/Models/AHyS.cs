using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AHSPersonDetection.MongoDB.Models
{
    public class AHyS
    {
        public ObjectId ID_DatosEntrada;
        public ObjectId ID_Local;
        public string Nombre_Lugar;
        public string Fecha;
        public int CantidadPersonas;
        public string? DirImagen; // Variable solo para escaneo
    }

}
