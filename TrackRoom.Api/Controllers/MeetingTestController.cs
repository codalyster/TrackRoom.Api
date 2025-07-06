using Microsoft.AspNetCore.Mvc;
using TrackRoom.Services.DTOs.Meeting;

namespace TrackRoom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingTestController : ControllerBase
    {
        private readonly IMeetingService meetingService;

        public MeetingTestController(IMeetingService meetingService)
        {
            this.meetingService = meetingService;
        }

        [HttpGet("test")]
        public async Task<IActionResult> TestMeeting()
        {
            try
            {
                var meetings = await meetingService.GetAllMeetingsAsync();

                List<MeetingDTO> meetingDtos = meetings.Select(m => new MeetingDTO
                {
                    //MeetingId = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    StartTime = m.StartTime,
                    EndTime = m.EndTime,
                    OrganizerId = m.OrganizerId,
                    OrganizerName = m.OrganizerName
                }).ToList();


                return Ok(meetingDtos);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving meetings.");
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateMeeting([FromBody] MeetingDTO meeting)
        {
            if (meeting == null)
            {
                return BadRequest("Meeting data is required.");
            }
            try
            {
                var meetingId = await meetingService.CreateMeetingAsync(meeting);
                return CreatedAtAction(nameof(CreateMeeting), new { id = meetingId }, new { meetingId, link = $"{Request.Scheme}://{Request.Host}/meeting/{meetingId}" });
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the meeting.");
            }
        }
    }
}
