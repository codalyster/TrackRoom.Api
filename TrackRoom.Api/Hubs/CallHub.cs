using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TrackRoom.DataAccess.Contexts;
using TrackRoom.DataAccess.Models;

namespace TrackRoom.Api.Hubs
{
    public class CallHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public CallHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task JoinMeeting(string meetingId)
        {
            if (string.IsNullOrWhiteSpace(meetingId))
                return;

            await Groups.AddToGroupAsync(Context.ConnectionId, meetingId);

            var userId = Context.UserIdentifier;
            var userName = Context.User?.Identity?.Name ?? "Unknown User";

            if (string.IsNullOrWhiteSpace(userId))
                return;

            var meeting = await _context.Meetings
                .Include(m => m.Organizer)
                .FirstOrDefaultAsync(m => m.Id == meetingId);

            if (meeting == null) return;

            var isOrganizer = meeting.OrganizerId == userId;

            var existingMember = await _context.Members
                .FirstOrDefaultAsync(m =>
                    m.ApplicationUserId == userId &&
                    m.MeetingId == meetingId &&
                    m.LeftAt == null);

            if (existingMember == null)
            {
                var newMember = new Member
                {
                    ApplicationUserId = userId,
                    MeetingId = meetingId,
                    JoinedAt = DateTime.UtcNow,
                    ConnectionId = Context.ConnectionId
                };

                _context.Members.Add(newMember);
                await _context.SaveChangesAsync();
            }

            // Get other participants
            var otherParticipants = await _context.Members
                .Include(m => m.ApplicationUser)
                .Where(m => m.MeetingId == meetingId && m.LeftAt == null && m.ApplicationUserId != userId)
                .Select(m => new
                {
                    m.ApplicationUserId,
                    m.ConnectionId,
                    FullName = m.ApplicationUser.FirstName + " " + m.ApplicationUser.LastName,
                    IsOrganizer = m.ApplicationUserId == meeting.OrganizerId
                })
                .ToListAsync();

            await Clients.Client(Context.ConnectionId).SendAsync("ExistingUsers", otherParticipants);

            await Clients.GroupExcept(meetingId, Context.ConnectionId).SendAsync("UserJoined", new
            {
                ConnectionId = Context.ConnectionId,
                UserId = userId,
                UserName = userName,
                IsOrganizer = isOrganizer
            });
        }

        public async Task LeaveMeeting(string meetingId)
        {
            if (string.IsNullOrWhiteSpace(meetingId))
                return;

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, meetingId);

            var userId = Context.UserIdentifier;
            if (string.IsNullOrWhiteSpace(userId))
                return;

            var member = await _context.Members
                .FirstOrDefaultAsync(m =>
                    m.ApplicationUserId == userId &&
                    m.MeetingId == meetingId &&
                    m.LeftAt == null);

            if (member != null)
            {
                member.LeftAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            var userName = Context.User?.Identity?.Name ?? "Unknown User";

            await Clients.Group(meetingId).SendAsync("UserLeft", new
            {
                ConnectionId = Context.ConnectionId,
                UserId = userId,
                UserName = userName
            });
        }

        public async Task EndMeeting(string meetingId)
        {
            var userId = Context.UserIdentifier;
            if (string.IsNullOrWhiteSpace(userId))
                return;

            var meeting = await _context.Meetings.FirstOrDefaultAsync(m => m.Id == meetingId);
            if (meeting == null || meeting.OrganizerId != userId)
                return; // Unauthorized

            var members = await _context.Members
                .Where(m => m.MeetingId == meetingId && m.LeftAt == null)
                .ToListAsync();

            foreach (var member in members)
                member.LeftAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            await Clients.Group(meetingId).SendAsync("MeetingEnded");

            foreach (var member in members)
            {
                await Groups.RemoveFromGroupAsync(member.ConnectionId, meetingId);
            }
        }

        public async Task KickUser(string meetingId, string targetConnectionId)
        {
            var userId = Context.UserIdentifier;
            if (string.IsNullOrWhiteSpace(userId))
                return;

            var meeting = await _context.Meetings.FirstOrDefaultAsync(m => m.Id == meetingId);
            if (meeting == null || meeting.OrganizerId != userId)
                return; // Only organizer can kick

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.MeetingId == meetingId && m.ConnectionId == targetConnectionId);

            if (member != null)
            {
                member.LeftAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                await Clients.Client(targetConnectionId).SendAsync("Kicked");
                await Groups.RemoveFromGroupAsync(targetConnectionId, meetingId);
            }
        }

        public async Task SendOffer(string meetingId, string targetConnectionId, string offer)
        {
            if (string.IsNullOrWhiteSpace(targetConnectionId) || string.IsNullOrWhiteSpace(offer))
                return;

            await Clients.Client(targetConnectionId).SendAsync("ReceiveOffer", Context.ConnectionId, offer);
        }

        public async Task SendAnswer(string meetingId, string targetConnectionId, string answer)
        {
            if (string.IsNullOrWhiteSpace(targetConnectionId) || string.IsNullOrWhiteSpace(answer))
                return;

            await Clients.Client(targetConnectionId).SendAsync("ReceiveAnswer", Context.ConnectionId, answer);
        }

        public async Task SendIceCandidate(string meetingId, string targetConnectionId, string candidate)
        {
            if (string.IsNullOrWhiteSpace(targetConnectionId) || string.IsNullOrWhiteSpace(candidate))
                return;

            await Clients.Client(targetConnectionId).SendAsync("ReceiveCandidate", Context.ConnectionId, candidate);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;

            // Try fallback if userId is null
            var member = userId != null
                ? await _context.Members.FirstOrDefaultAsync(m =>
                      m.ApplicationUserId == userId &&
                      m.LeftAt == null &&
                      m.ConnectionId == Context.ConnectionId)
                : await _context.Members.FirstOrDefaultAsync(m =>
                      m.LeftAt == null && m.ConnectionId == Context.ConnectionId);

            if (member != null)
            {
                member.LeftAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                await Clients.Group(member.MeetingId).SendAsync("UserLeft", new
                {
                    ConnectionId = Context.ConnectionId,
                    UserId = member.ApplicationUserId,
                    UserName = Context.User?.Identity?.Name ?? "Unknown User"
                });

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, member.MeetingId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
