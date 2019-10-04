using ShipBattleApi.Models.Enums;
using System.Threading.Tasks;

namespace ShipBattleAngularApi.BusinessLogic.Services.Interfaces
{
    public interface IHttpClientService
    {
        Task<bool> Join(string user, string pathBase);
        Task<bool> Start(string user, string enemy);
        Task Left(string user);
        Task<string> AddShip(string user, string infoShip);
        Task<bool> Ready(string user);
        Task<bool> Move(string user, int num, int x, int y);
        Task<StateShot> Shot(string user, int num, int x, int y);
        Task<bool> Fix(string user, int num, int x, int y);
    }
}
