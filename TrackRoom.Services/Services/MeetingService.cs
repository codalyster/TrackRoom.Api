using TrackRoom.DataAccess.IUnitOfWorks;
using TrackRoom.Services.DTOs.Meeting;

namespace TrackRoom.Services.Services
{
    public class MeetingService : IMeetingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public string CreateMeeting()
        {
            throw new NotImplementedException();
        }

        public MeetingDTO? FindMeetingById(string meetingId)
        {
            throw new NotImplementedException();
        }
    }


}
