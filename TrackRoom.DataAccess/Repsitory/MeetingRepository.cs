using TrackRoom.DataAccess.IRepository;
using TrackRoom.DataAccess.Models;

namespace TrackRoom.DataAccess.Repsitory
{
    public class MeetingRepository : IMeetingRepository
    {
        private readonly IRepository<Meeting> _meetingRepository;

        public MeetingRepository(IRepository<Meeting> meetingRepository)
        {
            _meetingRepository = meetingRepository;
        }

        public async Task<string> CreateMeetingAsync(Meeting meeting)
        {
            meeting.Id = Guid.NewGuid().ToString();

            await _meetingRepository.AddAsync(meeting);

            return meeting.Id;
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
            return _meetingRepository.GetAsync(m => m.Id == meetingId, tracked: false);
        }

        public Task<IEnumerable<Meeting>> GetAllMeetingsAsync()
        {
            return _meetingRepository.GetAllAsync();
        }

        public Task<bool> UpdateMeetingAsync(Meeting meeting)
        {
            try
            {
                _meetingRepository.Update(meeting);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                return Task.FromResult(false);
            }
        }
    }
}
