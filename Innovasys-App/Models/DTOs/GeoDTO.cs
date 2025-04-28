using System.ComponentModel.DataAnnotations;
using static Innovasys_App.Data.Constants.GlobalConstants;

namespace Innovasys_App.Models.DTOs
{
    public class GeoDto
    {
        [Required]
        [StringLength(LatAndLngLength)]
        public string? Lat { get; set; }

        [Required]
        [StringLength(LatAndLngLength)]
        public string? Lng { get; set; }
    }
}
