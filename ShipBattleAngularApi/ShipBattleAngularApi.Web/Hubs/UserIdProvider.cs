using Microsoft.AspNetCore.SignalR;

namespace ShipBattleAngularApi.Web.Hubs
{
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.Identity.Name;
        }
    }
}
