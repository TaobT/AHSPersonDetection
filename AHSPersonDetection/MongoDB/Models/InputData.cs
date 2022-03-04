using MongoDB.Bson.Serialization.Attributes;

namespace AHSPersonDetection.MongoDB.Models
{
    public class InputData
    {
        public string? Place { get; set; }
        public string? Date { get; set; }
        public string? Hour { get; set; }
        public string ImageUrl { get; set; }
        public bool Processed { get; set; } //IsProcessed

    }
}
