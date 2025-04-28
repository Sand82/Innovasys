using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Innovasys_App.Data.Constants.GlobalConstants;

namespace Innovasys_App.Models.DTOs
{
    public class UserDTO
    {
        [Required]
        [StringLength(UserNameLength, ErrorMessage = ErrorMassage)]
        public string? Name { get; set; }

        [Required]
        [StringLength(NotUsernameLength, ErrorMessage = ErrorMassage)]
        public string? Username { get; set; }

        [Required]
        [StringLength(EmailLength, ErrorMessage = ErrorMassage)]
        public string? Email { get; set; }

        [Required]
        [StringLength(PhoneLength, ErrorMessage = ErrorMassage)]
        public string? Phone { get; set; }

        [Required]
        [Column(MaxLength)]
        public string? Website { get; set; }

        [Required]
        [Column(MaxLength)]
        public AddressDTO? Address { get; set; }
    }
}
