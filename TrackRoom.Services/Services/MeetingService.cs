using System.Collections.Concurrent;
using TrackRoom.Services.DTOs.Meeting;

namespace TrackRoom.Services.Services
{
    public class MeetingService : IMeetingService
    {
        private static ConcurrentDictionary<string, MeetingDTO> _meetings = new();

        public string CreateMeeting()
        {
            var id = Guid.NewGuid().ToString();
            var meeting = new MeetingDTO
            {
                Id = id,
                StartTime = DateTime.UtcNow
            };

            _meetings.TryAdd(id, meeting);
            return id;
        }

        public MeetingDTO? FindMeetingById(string meetingId)
        {
            _meetings.TryGetValue(meetingId, out var meeting);
            return meeting;
        }
    }


}
