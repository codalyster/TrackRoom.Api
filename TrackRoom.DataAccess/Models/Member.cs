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

        [ForeignKey(nameof(Meeting))]
        public string MeetingId { get; set; } = null!;
        public Meeting Meeting { get; set; } = null!;

        public string ConnectionId { get; set; } = null!; // ✅ new field

    }
}
