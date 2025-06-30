using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TrackRoom.DataAccess.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public string? ProfilePictureUrl { get; set; }

        public string? Bio { get; set; }

        public string? Location { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

    }
}
