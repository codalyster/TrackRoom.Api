using Microsoft.AspNetCore.Mvc;
using TrackRoom.Services.DTOs.Meeting;


[ApiController]
[Route("api/[controller]")]
public class MeetingsController : ControllerBase
{
    private readonly IMeetingService _meetingService;
    private readonly string _frontendUrl;

    public MeetingsController(IMeetingService meetingService, IConfiguration config)
    {
        _meetingService = meetingService;
        _frontendUrl = config["Frontend:BaseUrl"];
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateMeetingAsync(MeetingDTO meeting)
    {
        var meetingId = await _meetingService.CreateMeetingAsync(meeting);
        return Ok(new { meetingId, link = $"{_frontendUrl}/meeting/{meetingId}" });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMeetingAsync(string id)
    {
        var meeting = await _meetingService.FindMeetingByIdAsync(id);
        if (meeting == null)
            return NotFound();

        return Ok(meeting);
    }

    [HttpGet("{meetingId}/participants")]
    public async Task<IActionResult> GetParticipants(string meetingId)
    {
        var meeting = await _meetingService.FindMeetingByIdAsync(meetingId);
        if (meeting == null) return NotFound();

        var participants = meeting.Members
            .Where(m => m.LeftAt == null)
            .Select(m => new
            {
                m.ApplicationUserId,
                m.ApplicationUser.FirstName,
                m.ApplicationUser.LastName,
                m.ConnectionId
            });

        return Ok(participants);
    }


}

