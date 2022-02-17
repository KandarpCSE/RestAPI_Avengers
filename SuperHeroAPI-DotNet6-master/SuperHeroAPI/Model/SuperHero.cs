using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace SuperHeroAPI
{
    public class SuperHero
    {
        [JsonIgnore]
        [Required]
        public int Id { get; set; }

        [Required]
        public string CharacterName { get; set; } = string.Empty;

        [Required]
        public string RealName { get; set; } = string.Empty;

        [Required]
        public string Power { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [JsonIgnore]
        public string ImageURl { get; set; } = "Not Uploaded" ;   
    }
}
