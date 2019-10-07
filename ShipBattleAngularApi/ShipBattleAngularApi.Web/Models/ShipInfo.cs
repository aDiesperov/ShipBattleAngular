using ShipBattleAngularApi.Web.Enums;

namespace ShipBattleAngularApi.Web.Models
{
    public class ShipInfo
    {
        public ShipInfo()
        {

        }

        public TypeShips TypeShip { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Dir { get; set; }
        public int Len { get; set; }
        public int Speed { get; set; }
        public int R_act { get; set; }
        public int Act1 { get; set; }
        public int Act2 { get; set; }
    }
}
