using ShipBattleAngularApi.BusinessLogic.Models;
using ShipBattleAngularApi.BusinessLogic.Services.Interfaces;
using ShipBattleApi.Models.Enums;
using ShipBattleApi.Models.Models;

namespace ShipBattleAngularApi.BusinessLogic.Services
{
    public class GameService : IGameService
    {
        private readonly IInfoGameService _infoGameService;

        public GameService(IInfoGameService infoGameService)
        {
            _infoGameService = infoGameService;
        }

        public void FixShip(InfoGame userInfo, int numShip, int broken)
        {
            ShipModel ship = userInfo.GameField.Coors[numShip].Ship;
            if (ship != null) ship.Broken = broken;
        }

        public void HitShip(InfoGame userInfo, int numShoted, double damage, bool died)
        {
            if (userInfo != null)
            {
                ShipModel ship = userInfo.GameField.Coors[numShoted].Ship;
                if (ship != null)
                {
                    ship.Broken += damage;

                    if (died) userInfo.GameField.DeadShips++;
                }
            }
        }

        public void Move(InfoGame userInfo, int num, int x, int y)
        {
            var gameField = userInfo.GameField;
            var coor = gameField.Coors[num];

            coor.X = x;
            coor.Y = y;
        }

        public void PrepareGame(InfoGame userInfo, GameFieldModel gameFieldModel, bool myQueue)
        {
            userInfo.State = StateReadyGame.Prepare;
            userInfo.GameField = gameFieldModel;
            userInfo.MyQueue = myQueue;
        }

        public void ResetGame(InfoGame userInfo)
        {
            userInfo.GameField = null;
            userInfo.Enemy = null;
            userInfo.MyQueue = false;
            userInfo.State = StateReadyGame.None;
        }
    }
}
