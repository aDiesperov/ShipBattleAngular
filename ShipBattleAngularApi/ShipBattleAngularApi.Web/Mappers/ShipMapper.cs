using ShipBattleAngularApi.Web.Models;
using ShipBattleApi.Models.Models;
using System;

namespace ShipBattleAngularApi.Web.Mappers
{
    public static class ShipMapper
    {
        public static ShipViewModel MapToShipViewModel(this ShipModel shipModel)
        {
            ShipViewModel ship = null;

            Type type = shipModel.GetType();
            switch (type.Name)
            {
                case nameof(MilitaryShipModel):
                    ship = new MilitaryShipViewModel(shipModel.Id, shipModel.Length, shipModel.Speed, shipModel.RadiusAction, shipModel.Direction, ((MilitaryShipModel)shipModel).Damage);
                    break;
                case nameof(SupportShipModel):
                    ship = new SupportShipViewModel(shipModel.Id, shipModel.Length, shipModel.Speed, shipModel.RadiusAction, shipModel.Direction, ((SupportShipModel)shipModel).Health);
                    break;
                case nameof(HybridShipModel):
                    ship = new HybridShipViewModel(shipModel.Id, shipModel.Length, shipModel.Speed, shipModel.RadiusAction, shipModel.Direction, ((HybridShipModel)shipModel).Damage, ((HybridShipModel)shipModel).Health);
                    break;
            }

            return ship;
        }
    }
}
