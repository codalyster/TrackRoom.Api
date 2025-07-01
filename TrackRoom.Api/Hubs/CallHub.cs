namespace TrackRoom.Api.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using TrackRoom.DataAccess.Contexts;
    using TrackRoom.DataAccess.Models;

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

            // تحقق لو المستخدم موجود مسبقًا بنفس الـ meeting
            var existingMember = await _context.Members
                .FirstOrDefaultAsync(m => m.ApplicationUserId == userId && m.MeetingId == meetingId && m.LeftAt == null);

            if (existingMember == null)
            {
                var newMember = new Member
                {
                    ApplicationUserId = userId,
                    MeetingId = meetingId,
                    JoinedAt = DateTime.UtcNow
                };

                _context.Members.Add(newMember);
                await _context.SaveChangesAsync();
            }

            await Clients.Group(meetingId).SendAsync("UserJoined", new
            {
                ConnectionId = Context.ConnectionId,
                UserId = userId
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

            await Clients.Group(meetingId).SendAsync("UserLeft", new
            {
                ConnectionId = Context.ConnectionId,
                UserId = userId
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
            // ممكن تطور الجزء ده لاحقًا عشان تمسك آخر اجتماع للمستخدم وتحدث LeftAt تلقائيًا
            await base.OnDisconnectedAsync(exception);
        }
    }



}

