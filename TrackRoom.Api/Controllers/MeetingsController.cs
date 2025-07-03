//using Microsoft.AspNetCore.Mvc;

//[ApiController]
//[Route("api/[controller]")]
//public class MeetingsController : ControllerBase
//{
//    private readonly IMeetingService _meetingService;
//    private readonly string _frontendUrl;

//    public MeetingsController(IMeetingService meetingService, IConfiguration config)
//    {
//        _meetingService = meetingService;
//        _frontendUrl = config["Frontend:BaseUrl"];
//    }

//    [HttpPost("create")]
//    //[Authorize]
//    public IActionResult CreateMeeting()
//    {
//        var meetingId = _meetingService.CreateMeeting();
//        return Ok(new { meetingId, link = $"{_frontendUrl}/meeting/{meetingId}" });
//    }


//    [HttpGet("{id}")]
//    //[Authorize]
//    public IActionResult GetMeeting(string id)
//    {
//        var meeting = _meetingService.FindMeetingById(id);
//        if (meeting == null)
//            return NotFound();

//        return Ok(meeting);
//    }

//}

