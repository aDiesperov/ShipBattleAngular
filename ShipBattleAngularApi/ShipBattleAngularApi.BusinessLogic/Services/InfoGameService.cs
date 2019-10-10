using ShipBattleAngularApi.BusinessLogic.Models;
using ShipBattleAngularApi.BusinessLogic.Services.Interfaces;
using ShipBattleApi.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipBattleAngularApi.BusinessLogic.Services
{
    public class InfoGameService : IInfoGameService
    {
        private class ExtendedInfoGame : InfoGame
        {
            public ExtendedInfoGame(string connectionId)
            {
                ConnectionId = connectionId;
            }
            public KeyValuePair<string, ExtendedInfoGame> Self { get; set; }
        }

        private Dictionary<string, ExtendedInfoGame> _userInfoGame;

        public InfoGameService()
        {
            _userInfoGame = new Dictionary<string, ExtendedInfoGame>();
        }

        public bool Add(string user, string connectionId)
        {
            if (Exists(user)) return false;

            _userInfoGame.Add(user, new ExtendedInfoGame(connectionId));
            _userInfoGame.Last().Value.Self = _userInfoGame.Last();

            return true;
        }

        public bool Exists(string user)
        {
            if (String.IsNullOrEmpty(user)) return false;
            return _userInfoGame.ContainsKey(user);
        }

        public InfoGame this[string name]
        {
            get => Exists(name) ? _userInfoGame[name] : null;
        }

        public void Remove(string user)
        {
            if (Exists(user)) _userInfoGame.Remove(user);
        }

        public InfoGame GetUserByConnectionId(string connectionId) => _userInfoGame.SingleOrDefault(kvp => kvp.Value.ConnectionId == connectionId).Value;

        public bool ShipsExist(InfoGame userInfo) => userInfo.GameField.Coors.Any();

        public string GetName(InfoGame infoGame) => ((ExtendedInfoGame)infoGame).Self.Key;
    }
}
