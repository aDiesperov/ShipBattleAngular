using EncryptionAttributes.Attributes;
using ShipBattleApi.Models.Models;

namespace ShipBattleAngularApi.Web.Models
{
    public class CoorViewModel
    {
        [Encryption("EncryptionAttributes.Services.EncryptionService", "P@@Sw0rd", "S@LT&KEY", "@1B2c3D4e5F6g7H8")]
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public ShipViewModel Ship { get; set; }
        public int Direction { get; set; }

        public CoorViewModel() { }

        public CoorViewModel(ShipViewModel ship, int id, int x, int y, int direction)
        {
            Id = id;
            X = x;
            Y = y;
            Direction = direction;
            Ship = ship;
        }

        public CoorModel MapToCoorModel()
        {
            ShipModel shipModel = Ship.MapToShipModel();
            CoorModel coorModel = new CoorModel(Id, X, Y, Direction, shipModel);
            return coorModel;
        }
    }
}
