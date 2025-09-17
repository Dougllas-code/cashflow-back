using CashFlow.Domain.Services.LoggedUser;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace CashFlow.Api.HubMethods
{
    public class SessionHub : Hub
    {
        private readonly ILoggedUser _loggedUser;
        private static readonly ConcurrentDictionary<Guid, HashSet<string>> UserConnections = new();

        public SessionHub(ILoggedUser loggedUser)
        {
            _loggedUser = loggedUser;
        }

        public override async Task OnConnectedAsync()
        {
            var user = await _loggedUser.Get();
            if (user != null)
            {
                var connections = UserConnections.GetOrAdd(user.UserIdentifier, _ => new HashSet<string>());
                lock (connections)
                {
                    connections.Add(Context.ConnectionId);
                }
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(System.Exception? exception)
        {
            var user = await _loggedUser.Get();
            if (user != null && UserConnections.TryGetValue(user.UserIdentifier, out var connections))
            {
                lock (connections)
                {
                    connections.Remove(Context.ConnectionId);
                    if (connections.Count == 0)
                        UserConnections.TryRemove(user.UserIdentifier, out _);
                }
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task ForceLogoutPreviousSessions()
        {
            var user = await _loggedUser.Get();
            if (user != null && UserConnections.TryGetValue(user.UserIdentifier, out var connections))
            {
                var connectionsToLogout = connections.Where(id => id != Context.ConnectionId).ToList();
                if (connectionsToLogout.Any())
                {
                    await Clients.Clients(connectionsToLogout).SendAsync("ForceLogout", $"User {user.Email} has been logged out from other session.");
                }
            }
        }
       
    }
}
