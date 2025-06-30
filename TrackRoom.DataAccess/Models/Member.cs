using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackRoom.DataAccess.Models
{
    public class Member
    {
        [Key]
        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; } = null!;

        public DateTime? JoinedAt { get; set; }

        public DateTime? LeftAt { get; set; }

        public string MeetingId { get; set; } = null!;

    }
}
