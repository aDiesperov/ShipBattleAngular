using Microsoft.Extensions.DependencyInjection;
using ShipBattleAngularApi.BusinessLogic.Services;
using ShipBattleAngularApi.BusinessLogic.Services.Interfaces;

namespace ShipBattleAngularApi.BusinessLogic.ExtensionsDI
{
    public static class HttpClientServiceExtension
    {
        public static void AddHttpClientService(this IServiceCollection services)
        {
            services.AddSingleton<IHttpClientService, HttpClientService>();
        }
    }
}
