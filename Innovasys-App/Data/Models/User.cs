using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static Innovasys_App.Data.Constants.GlobalConstants;

namespace Innovasys_App.Data.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [StringLength(UserNameLength)]
        public string? Name { get; set; }

        [StringLength(NotUsernameLength)]
        public string? NotUsername { get; set; }

        [StringLength(EmailLength)]
        public string? Email { get; set; }

        [StringLength(PhoneLength)]
        public string? Phone { get; set; }

        [Column(TypeName = MaxLength)]
        public string? Website { get; set; }

        [Column(TypeName = MaxLength)]
        public string? Note { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public Address? Address { get; set; }
    }
}
