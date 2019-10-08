using ShipBattleAngularApi.BusinessLogic.Models;
using ShipBattleAngularApi.BusinessLogic.Services.Interfaces;
using ShipBattleApi.Models.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShipBattleAngularApi.BusinessLogic.Services
{
    public class InfoGameService : IInfoGameService
    {
        private Dictionary<string, InfoGame> _userInfoGame;

        public InfoGameService()
        {
            _userInfoGame = new Dictionary<string, InfoGame>();
        }

        public bool Add(string user, string connectionId)
        {
            if (Exists(user)) return false;

            _userInfoGame.Add(user, new InfoGame(connectionId));

            return true;
        }

        public bool Exists(string user) => _userInfoGame.ContainsKey(user);

        public IEnumerable<string> AllConnections
        {
            get => _userInfoGame.Values.Select(ig => ig.ConnectionId).Distinct();
        }

        public InfoGame this[string name]
        {
            get
            {
                if (Exists(name))
                    return _userInfoGame[name];
                return null;
            }
        }

        public void Remove(string user)
        {
            if (Exists(user)) _userInfoGame.Remove(user);
        }

        public string GetUserByConnectionId(string connectionId) =>_userInfoGame.SingleOrDefault(kvp => kvp.Value.ConnectionId == connectionId).Key;

        public bool ShipsExist(string user)
        {
            if (Exists(user))
            {
                return _userInfoGame[user].GameField.Coors.Any();
            }
            return false;
        }

        public IEnumerable<CoorModel> GetCoors(string user)
        {
            if (Exists(user))
            {
                return _userInfoGame[user].GameField.Coors;
            }
            return new List<CoorModel>();
        }

        public string GetConnectionId(string user)
        {
            if (Exists(user))
            {
                return _userInfoGame[user].ConnectionId;
            }
            return null;
        }

        public GameFieldModel GetField(string user)
        {
            if (Exists(user))
            {
                return _userInfoGame[user].GameField;
            }
            return null;
        }
    }
}
