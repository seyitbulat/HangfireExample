using Microsoft.AspNetCore.SignalR.Client;
using System.Text.Json;

namespace TestService
{
    public class CurrencyTrigger
    {
        public HubConnection connection { get; set; }
        private readonly CurrencyService currencyService;
        public CurrencyTrigger(CurrencyService currencyService)
        {
            connection = new HubConnectionBuilder().WithUrl("https://localhost:44384/currency").Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };
            this.currencyService = currencyService;
        }

        public async Task GetCurrencies()
        {
            await connection.StartAsync();

            var msg = await currencyService.GetCurrencies();
            var message = new SocketMessage<List<Currency>>()
            {
                Message = msg,
                Time = DateTime.Now.ToShortTimeString()
            };
            try
            {
                await connection.InvokeAsync("ReceiveMessage", JsonSerializer.Serialize(message));
            }
            catch (Exception ex)
            {
            }
        }
    }
}
