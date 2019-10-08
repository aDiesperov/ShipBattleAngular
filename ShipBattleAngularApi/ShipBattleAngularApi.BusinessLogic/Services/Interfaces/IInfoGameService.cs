using ShipBattleAngularApi.BusinessLogic.Models;
using ShipBattleApi.Models.Models;
using System.Collections.Generic;

namespace ShipBattleAngularApi.BusinessLogic.Services.Interfaces
{
    public interface IInfoGameService
    {
        bool Exists(string user);
        bool Add(string user, string connectionId);
        IEnumerable<string> AllConnections { get; }
        void Remove(string user);
        InfoGame this[string name] { get; }
        string GetUserByConnectionId(string connectionId);
        bool ShipsExist(string user);
        IEnumerable<CoorModel> GetCoors(string user);
        string GetConnectionId(string user);
        GameFieldModel GetField(string user);


    }
}
