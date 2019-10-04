using Microsoft.Extensions.DependencyInjection;
using ShipBattleAngularApi.BusinessLogic.Services;
using ShipBattleAngularApi.BusinessLogic.Services.Interfaces;

namespace ShipBattleAngularApi.BusinessLogic.ExtensionsDI
{
    public static class InfoGameServiceExtension
    {
        public static void AddInfoGameService(this IServiceCollection services)
        {
            services.AddSingleton<IInfoGameService, InfoGameService>();
        }
    }
}
