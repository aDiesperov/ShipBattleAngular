using ShipBattleAngularApi.BusinessLogic.Models;
using ShipBattleApi.Models.Models;

namespace ShipBattleAngularApi.BusinessLogic.Services.Interfaces
{
    public interface IGameService
    {
        void PrepareGame(InfoGame user, GameFieldModel gameFieldModel, bool myQueue);
        void ResetGame(InfoGame user);
        void HitShip(InfoGame user, int numShoted, int damage, bool died);
        void Move(InfoGame user, int num, int x, int y);
        void FixShip(InfoGame user, int numShip, int broken);
    }
}
