using TrackRoom.DataAccess.Models;
using TrackRoom.Services.DTOs.Meeting;

public interface IMeetingService
{
    Task<string> CreateMeetingAsync(MeetingDTO meeting);
    Task<Meeting?> FindMeetingByIdAsync(string meetingId);
    Task<IEnumerable<Meeting>> GetAllMeetingsAsync();
    Task<bool> UpdateMeetingAsync(MeetingDTO meeting);
    Task<bool> DeleteMeetingAsync(string meetingId);
}
