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

        public void HitShip(string user, int id, double damage, bool died)
        {
            var userInfo = _infoGameService[user];
            if (userInfo != null)
            {
                ShipModel ship = _infoGameService.GetShipById(user, id);
                if (ship != null)
                {
                    ship.Broken += damage;

                    if (died) userInfo.GameField.DeadShips++;
                }
            }
        }

        public bool Move(string user, int num, int x, int y)
        {
            if (_infoGameService.Exists(user))
            {
                var gameField = _infoGameService.GetField(user);
                var coor = gameField.Coors[num];

                coor.X = x;
                coor.Y = y;

                return true;
            }
            return false;
        }

        public bool PrepareGame(string user, GameFieldModel gameFieldModel, bool myQueue)
        {
            if (_infoGameService.Exists(user))
            {
                _infoGameService[user].State = StateReadyGame.Prepare;
                _infoGameService[user].GameField = gameFieldModel;
                _infoGameService[user].MyQueue = myQueue;
                return true;
            }
            return false;
        }

        public void ResetGame(string user)
        {
            if (_infoGameService.Exists(user))
            {
                var userInfo = _infoGameService[user];

                userInfo.GameField = null;
                userInfo.Enemy = null;
                userInfo.MyQueue = false;
                userInfo.State = StateReadyGame.None;
            }
        }

        public bool StartedGame(string user)
        {
            if (_infoGameService.Exists(user))
            {
                var userInfo = _infoGameService[user];
                userInfo.State = StateReadyGame.Started;
                return true;
            }
            return false;
        }
    }
}
