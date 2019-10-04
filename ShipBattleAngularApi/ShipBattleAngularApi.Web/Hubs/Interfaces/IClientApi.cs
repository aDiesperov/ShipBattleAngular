using System.Threading.Tasks;

namespace ShipBattleAngularApi.Web.Hubs.Interfaces
{
    public interface IClientApi
    {
        Task ReceiveMessage(string text, bool error);
        Task PrepareGame(int idField, string enemy);
    }
}
