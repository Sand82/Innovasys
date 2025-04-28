using System.ComponentModel.DataAnnotations;

namespace Innovasys_App.Models.Views
{
    public class UserViewModel
    {
        public int Id { get; set; }
        
        public string? Name { get; set; }
        
        public string? NotUsername { get; set; }
        
        public string? Email { get; set; }
        
        public string? Phone { get; set; }
        
        public string? Website { get; set; }
        
        public string? Note { get; set; }
        
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public AddressViewModel? Address { get; set; }
    }
}
