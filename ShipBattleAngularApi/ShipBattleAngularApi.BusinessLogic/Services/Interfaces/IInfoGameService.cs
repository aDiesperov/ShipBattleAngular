using ShipBattleAngularApi.BusinessLogic.Models;
using ShipBattleApi.Models.Models;
using System.Collections.Generic;

namespace ShipBattleAngularApi.BusinessLogic.Services.Interfaces
{
    public interface IInfoGameService
    {
        bool Exists(string user);
        bool Add(string user, string connectionId);
        void Remove(string user);
        InfoGame this[string name] { get; }
        InfoGame GetUserByConnectionId(string connectionId);
        bool ShipsExist(InfoGame user);
        string GetName(InfoGame infoGame);
    }
}
