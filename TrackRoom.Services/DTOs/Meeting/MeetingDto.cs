namespace TrackRoom.Services.DTOs.Meeting
{
    public class MeetingDTO
    {
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string OrganizerId { get; set; } = null!;

        public string OrganizerName { get; set; } = null!;

    }
}
