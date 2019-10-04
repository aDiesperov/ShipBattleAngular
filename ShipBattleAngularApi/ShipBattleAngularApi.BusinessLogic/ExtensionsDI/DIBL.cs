using Microsoft.Extensions.DependencyInjection;

namespace ShipBattleAngularApi.BusinessLogic.ExtensionsDI
{
    public static class DIBL
    {
        public static void AddDIBL(this IServiceCollection services)
        {
            services.AddGameService();
            services.AddHttpClientService();
            services.AddInfoGameService();
        }
    }
}
