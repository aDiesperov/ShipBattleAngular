using System.Threading.Tasks;

namespace ShipBattleAngularApi.Web.Hubs.Interfaces
{
    public interface IClientApi
    {
        Task ReceiveMessage(string text, bool error);
        Task Hit(string text, int num, double damageShoted, bool died);
        Task Fix(int numShip, int broken);
        Task PrepareGame(int id);
        Task Offer(string enemy);
        Task NextStep();
    }
}
