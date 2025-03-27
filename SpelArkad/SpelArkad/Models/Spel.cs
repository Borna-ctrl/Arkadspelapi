// Models/Spel.cs

namespace SpelArkad.Models
{


using System.Text.Json.Serialization;

    public class Spel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("titel")]
        public string Titel { get; set; }

        [JsonPropertyName("kategori")]
        public string Kategori { get; set; }

        [JsonPropertyName("beskrivning")]
        public string Beskrivning { get; set; }

        [JsonPropertyName("bildUrl")]
        public string BildUrl { get; set; }

        [JsonPropertyName("trailerUrl")]
        public string TrailerUrl { get; set; }
    }
}
