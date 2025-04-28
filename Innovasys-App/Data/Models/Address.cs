using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static Innovasys_App.Data.Constants.GlobalConstants;

namespace Innovasys_App.Data.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [StringLength(StreetLength)]
        public string? Street { get; set; }

        [StringLength(SuiteLength)]
        public string? Suite { get; set; }

        [StringLength(CityLength)]
        public string? City { get; set; }

        [StringLength(ZipCodeLength)]
        public string? ZipCode { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public User? User { get; set; }
    }
}
