using TrackRoom.DataAccess.Models;
namespace TrackRoom.DataAccess.IRepository
{
    public interface IMeetingRepository
    {
        Task<string> CreateMeetingAsync(Meeting meeting);
        Task<Meeting?> FindMeetingByIdAsync(string meetingId);
        Task<IEnumerable<Meeting>> GetAllMeetingsAsync();
        Task<bool> UpdateMeetingAsync(Meeting meeting);
        Task<bool> DeleteMeetingAsync(string meetingId);
    }
}
