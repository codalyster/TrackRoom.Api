using TrackRoom.Services.DTOs.Meeting;

public interface IMeetingService
{
    string CreateMeeting();
    MeetingDTO? FindMeetingById(string meetingId);
}
