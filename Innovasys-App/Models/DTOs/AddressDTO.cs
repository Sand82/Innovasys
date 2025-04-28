using System.ComponentModel.DataAnnotations;
using static Innovasys_App.Data.Constants.GlobalConstants;

namespace Innovasys_App.Models.DTOs
{
    public class AddressDTO
    {
        [Required]
        [StringLength(StreetLength, ErrorMessage = ErrorMassage)]
        public string? Street { get; set; }

        [Required]
        [StringLength(SuiteLength, ErrorMessage = ErrorMassage)]
        public string? Suite { get; set; }

        [Required]
        [StringLength(CityLength, ErrorMessage = ErrorMassage)]
        public string? City { get; set; }

        [Required]
        [StringLength(ZipCodeLength, ErrorMessage = ErrorMassage)]
        public string? ZipCode { get; set; }

        public GeoDto? Geo { get; set; }
    }
}
