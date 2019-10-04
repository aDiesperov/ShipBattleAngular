using Microsoft.Extensions.DependencyInjection;
using ShipBattleAngularApi.BusinessLogic.Services;
using ShipBattleAngularApi.BusinessLogic.Services.Interfaces;

namespace ShipBattleAngularApi.BusinessLogic.ExtensionsDI
{
    public static class GameServiceExtension
    {
        public static void AddGameService(this IServiceCollection services)
        {
            services.AddScoped<IGameService, GameService>();
        }
    }
}
