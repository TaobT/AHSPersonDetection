using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHSPersonDetection.MongoDB.Models
{
    public class AHySP
    {
        public int ID_Entrada;
        public int ID_Lugar;
        public string Nombre_Lugar;
        public DateTime Fecha;
        public int CantidadPersonas;
        public string UrlImagen;
        public string? DirImagen; // Variable solo para escaneo
    }
}
