using TrackRoom.DataAccess.Models;
using TrackRoom.Services.DTOs.Meeting;

namespace TrackRoom.Services.Services
{
    public class MeetingService : IMeetingService
    {
        private readonly IRepository<Meeting> _meetingRepository;


        public MeetingService(IRepository<Meeting> meetingRepository)
        {
            _meetingRepository = meetingRepository;
        }

        public async Task<string> CreateMeetingAsync(MeetingDTO meeting)
        {
            try
            {
                var newMeeting = new Meeting
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = meeting.Title,
                    Description = meeting.Description,
                    StartTime = meeting.StartTime,
                    EndTime = meeting.EndTime,
                    OrganizerId = meeting.OrganizerId,
                };
                await _meetingRepository.AddAsync(newMeeting);
                return newMeeting.Id;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                return string.Empty;
            }
        }

        public async Task<bool> DeleteMeetingAsync(string meetingId)
        {
            try
            {
                var meeting = await _meetingRepository.GetAsync(m => m.Id == meetingId, tracked: true);

                if (meeting != null)
                {
                    _meetingRepository.Delete(meeting);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                return false;
            }
        }



        public Task<Meeting?> FindMeetingByIdAsync(string meetingId)
        {
            return _meetingRepository.GetAsync(m => m.Id == meetingId, tracked: false, includeProperties: "Organizer");
        }

        public Task<IEnumerable<Meeting>> GetAllMeetingsAsync()
        {
            return _meetingRepository.GetAllAsync(includeProperties: "Organizer");
        }

        public async Task<bool> UpdateMeetingAsync(MeetingDTO meeting)
        {
            try
            {
                var existingMeeting = await _meetingRepository.GetAsync(m => m.Id == meeting.Id, tracked: true);

                if (existingMeeting == null) return false;

                existingMeeting.Title = meeting.Title;
                existingMeeting.Description = meeting.Description;
                existingMeeting.StartTime = meeting.StartTime;
                existingMeeting.EndTime = meeting.EndTime;

                _meetingRepository.Update(existingMeeting);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
