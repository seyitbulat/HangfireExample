using Microsoft.AspNetCore.SignalR;

namespace HangFireTest.Hub
{
    public class CurrencyHub : Microsoft.AspNetCore.SignalR.Hub
    {

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("SendMessage",message);
        }
        public async Task ReceiveMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
