using System.Threading.Tasks;

namespace ShipBattleAngularApi.Web.Hubs.Interfaces
{
    public interface IClientApi
    {
        Task ReceiveMessage(string text, bool error);
        Task Hit(string text, int num, double damageShoted, bool died);
        Task PrepareGame(int idField, string enemy);
        Task Fix(int numShip, int broken);
    }
}
