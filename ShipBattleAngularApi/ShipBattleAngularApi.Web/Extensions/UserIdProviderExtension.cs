using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using ShipBattleAngularApi.Web.Hubs;

namespace ShipBattleAngularApi.Web.Extensions
{
    public static class UserIdProviderExtension
    {
        public static void AddUserIdProvider(this IServiceCollection services)
        {
            services.AddSingleton<IUserIdProvider, UserIdProvider>();
        }
    }
}
