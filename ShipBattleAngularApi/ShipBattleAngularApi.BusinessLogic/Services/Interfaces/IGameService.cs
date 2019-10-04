using ShipBattleApi.Models.Models;

namespace ShipBattleAngularApi.BusinessLogic.Services.Interfaces
{
    public interface IGameService
    {
        bool PrepareGame(string user, GameFieldModel gameFieldModel, bool myQueue);
        bool StartedGame(string user);
        void ResetGame(string user);
        void HitShip(string user, int id, double damage, bool died);
        bool Move(string user, int num, int x, int y);
    }
}
