using ShipBattleApi.Models.Enums;
using ShipBattleApi.Models.Models;

namespace ShipBattleAngularApi.BusinessLogic.Models
{
    public class InfoGame
    {
        public InfoGame(string connectionId)
        {
            ConnectionId = connectionId;
        }
        public string ConnectionId { get; set; }
        public string Enemy { get; set; }
        public GameFieldModel GameField { get; set; }
        public StateReadyGame State { get; set; }
        public bool MyQueue { get; set; }
    }
}
