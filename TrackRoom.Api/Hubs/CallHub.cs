namespace TrackRoom.Api.Hubs
{
    using Microsoft.AspNetCore.SignalR;

    public class CallHub : Hub
    {
        public async Task JoinMeeting(string meetingId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, meetingId);
            await Clients.Group(meetingId).SendAsync("UserJoined", Context.ConnectionId);
        }

        public async Task LeaveMeeting(string meetingId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, meetingId);
            await Clients.Group(meetingId).SendAsync("UserLeft", Context.ConnectionId);
        }

        public async Task SendOffer(string meetingId, string targetConnectionId, string offer)
        {
            await Clients.Client(targetConnectionId).SendAsync("ReceiveOffer", Context.ConnectionId, offer);
        }

        public async Task SendAnswer(string meetingId, string targetConnectionId, string answer)
        {
            await Clients.Client(targetConnectionId).SendAsync("ReceiveAnswer", Context.ConnectionId, answer);
        }

        public async Task SendCandidate(string meetingId, string targetConnectionId, string candidate)
        {
            await Clients.Client(targetConnectionId).SendAsync("ReceiveCandidate", Context.ConnectionId, candidate);
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
