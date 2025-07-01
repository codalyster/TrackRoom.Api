namespace TrackRoom.Api.Hubs
{
    using Microsoft.AspNetCore.SignalR;

    public class CallHub : Hub
    {
        /// <summary>
        /// Adds the user to a SignalR group for the meeting and notifies others.
        /// </summary>
        public async Task JoinMeeting(string meetingId)
        {
            if (string.IsNullOrWhiteSpace(meetingId))
                return;

            await Groups.AddToGroupAsync(Context.ConnectionId, meetingId);

            var userId = Context.UserIdentifier ?? "Anonymous";
            var userName = Context.User?.Identity?.Name ?? "Unknown User";
            await Clients.Group(meetingId).SendAsync(userName, new
            {
                ConnectionId = Context.ConnectionId,
                UserId = userId
            });
        }

        /// <summary>
        /// Removes the user from the meeting group and notifies others.
        /// </summary>
        public async Task LeaveMeeting(string meetingId)
        {
            if (string.IsNullOrWhiteSpace(meetingId))
                return;

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, meetingId);

            var userId = Context.UserIdentifier ?? "Anonymous";
            await Clients.Group(meetingId).SendAsync("UserLeft", new
            {
                ConnectionId = Context.ConnectionId,
                UserId = userId
            });
        }

        /// <summary>
        /// Sends a WebRTC offer to a specific user in the meeting.
        /// </summary>
        public async Task SendOffer(string meetingId, string targetConnectionId, string offer)
        {
            if (string.IsNullOrWhiteSpace(targetConnectionId) || string.IsNullOrWhiteSpace(offer))
                return;

            await Clients.Client(targetConnectionId).SendAsync("ReceiveOffer", Context.ConnectionId, offer);
        }

        /// <summary>
        /// Sends a WebRTC answer to a specific user in the meeting.
        /// </summary>
        public async Task SendAnswer(string meetingId, string targetConnectionId, string answer)
        {
            if (string.IsNullOrWhiteSpace(targetConnectionId) || string.IsNullOrWhiteSpace(answer))
                return;

            await Clients.Client(targetConnectionId).SendAsync("ReceiveAnswer", Context.ConnectionId, answer);
        }

        /// <summary>
        /// Sends an ICE candidate to a specific user in the meeting.
        /// </summary>
        public async Task SendCandidate(string meetingId, string targetConnectionId, string candidate)
        {
            if (string.IsNullOrWhiteSpace(targetConnectionId) || string.IsNullOrWhiteSpace(candidate))
                return;

            await Clients.Client(targetConnectionId).SendAsync("ReceiveCandidate", Context.ConnectionId, candidate);
        }

        /// <summary>
        /// (Optional) Override when user disconnects.
        /// </summary>
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // ممكن تضيف لوجيك لتسجيل خروج المستخدم تلقائيًا من الميتنج.
            await base.OnDisconnectedAsync(exception);
        }

        //public override Task OnConnectedAsync()
        //{
        //    return base.OnConnectedAsync();
        //}
        //public override Task OnDisconnectedAsync(Exception? exception)
        //{
        //    return base.OnDisconnectedAsync(exception);
        //}
    }


}

