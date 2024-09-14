using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace DataStreamHub
{
    public class DataStreamHub : Hub
    {
        private readonly ILogger _logger; 
        public DataStreamHub(ILoggerFactory loggerFactory)
        {
            _logger=loggerFactory.CreateLogger("DataStreamHub");
        }
        
        public async Task SendMessage(string clientName, string clientMessage)
        {
            _logger.LogInformation($"Hub Message forwarded Client {clientName} : Message {clientMessage}");
            await Clients.All.SendAsync("Pi Data Message", clientName, clientMessage);
            
        }
    }

}


