using System.ComponentModel.DataAnnotations.Schema;

namespace TrackRoom.DataAccess.Models
{
    public class Meeting
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        [ForeignKey(nameof(Organizer))]
        public string OrganizerId { get; set; } = null!;

        public ApplicationUser Organizer { get; set; } = null!;

        public string OrganizerName => $"{Organizer.FirstName} {Organizer.LastName}";

        public ICollection<Member> Members { get; set; } = new List<Member>();

    }
}
