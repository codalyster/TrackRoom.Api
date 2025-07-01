using Microsoft.AspNetCore.SignalR;

namespace TrackRoom.Api.Hubs
{
    public class CallHub : Hub
    {
        public async Task SendOffer(string user, string sdp)
            => await Clients.User(user).SendAsync("ReceiveOffer", sdp);

        public async Task SendAnswer(string user, string sdp)
            => await Clients.User(user).SendAsync("ReceiveAnswer", sdp);

        public async Task SendCandidate(string user, string candidate)
            => await Clients.User(user).SendAsync("ReceiveCandidate", candidate);
    }
}
