using ShipBattleAngularApi.Web.Models;
using ShipBattleApi.Models.Models;

namespace ShipBattleAngularApi.Web.Mappers
{
    public static class CoorMapper
    {
        public static CoorViewModel MapToCoorViewModel(this CoorModel coorModel)
        {
            ShipViewModel shipViewModel = coorModel.Ship.MapToShipViewModel();
            return new CoorViewModel(shipViewModel, coorModel.Id, coorModel.X, coorModel.Y, coorModel.Direction);
        }
    }
}
