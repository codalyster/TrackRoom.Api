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
            if (userId == null)
                return;

            var userName = Context.User?.Identity?.Name ?? "Unknown User";

            // Check if the user is already joined (active)
            var existingMember = await _context.Members
                .FirstOrDefaultAsync(m => m.ApplicationUserId == userId && m.MeetingId == meetingId && m.LeftAt == null);

            if (existingMember == null)
            {
                var newMember = new Member
                {
                    ApplicationUserId = userId,
                    MeetingId = meetingId,
                    JoinedAt = DateTime.UtcNow,
                    ConnectionId = Context.ConnectionId // ✅ Store connection ID
                };

                _context.Members.Add(newMember);
                await _context.SaveChangesAsync();
            }

            // Send existing participants to the newly joined user
            var otherParticipants = await _context.Members
                .Where(m => m.MeetingId == meetingId && m.LeftAt == null && m.ApplicationUserId != userId)
                .Select(m => new
                {
                    m.ApplicationUserId,
                    m.ConnectionId
                })
                .ToListAsync();

            await Clients.Client(Context.ConnectionId).SendAsync("ExistingUsers", otherParticipants);

            // Notify others in the group about the new user
            await Clients.GroupExcept(meetingId, Context.ConnectionId).SendAsync("UserJoined", new
            {
                ConnectionId = Context.ConnectionId,
                UserId = userId,
                UserName = userName
            });
        }

        public async Task LeaveMeeting(string meetingId)
        {
            if (string.IsNullOrWhiteSpace(meetingId))
                return;

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, meetingId);

            var userId = Context.UserIdentifier;
            if (userId == null)
                return;

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.ApplicationUserId == userId && m.MeetingId == meetingId && m.LeftAt == null);

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

        public async Task SendCandidate(string meetingId, string targetConnectionId, string candidate)
        {
            if (string.IsNullOrWhiteSpace(targetConnectionId) || string.IsNullOrWhiteSpace(candidate))
                return;

            await Clients.Client(targetConnectionId).SendAsync("ReceiveCandidate", Context.ConnectionId, candidate);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Try to find and mark the user as disconnected
            var userId = Context.UserIdentifier;
            if (userId != null)
            {
                var activeMember = await _context.Members
                    .FirstOrDefaultAsync(m => m.ApplicationUserId == userId && m.LeftAt == null && m.ConnectionId == Context.ConnectionId);

                if (activeMember != null)
                {
                    activeMember.LeftAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();

                    await Clients.Group(activeMember.MeetingId).SendAsync("UserLeft", new
                    {
                        ConnectionId = Context.ConnectionId,
                        UserId = userId,
                        UserName = Context.User?.Identity?.Name ?? "Unknown User"
                    });

                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, activeMember.MeetingId);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
