using System;
using EncryptionAttributes.Attributes;
using ShipBattleApi.Models.Models;

namespace ShipBattleAngularApi.Web.Models
{
    public class ShipViewModel
    {
        [Encryption("EncryptionAttributes.Services.EncryptionService", "P@@Sw0rd", "S@LT&KEY", "@1B2c3D4e5F6g7H8")]
        public int Id { get; set; }
        public int Length { get; set; }
        public int Speed { get; set; }
        public int RadiusAction { get; set; }
        public int Direction { get; set; }

        public ShipViewModel(int length, int speed, int radiusAction)
        {
            Length = length;
            Speed = speed;
            RadiusAction = radiusAction;
        }

        public ShipViewModel(int id, int length, int speed, int radiusAction, int direction)
        {
            Length = length;
            Speed = speed;
            RadiusAction = radiusAction;
            Direction = direction;
            Id = id;
        }

        public ShipModel MapToShipModel()
        {
            ShipModel shipModel = null;

            Type type = GetType();
            switch (type.Name)
            {
                case nameof(MilitaryShipViewModel):
                    shipModel = new MilitaryShipModel(Id, Length, Speed, RadiusAction, Direction, ((MilitaryShipViewModel)this).Damage);
                    break;
                case nameof(SupportShipViewModel):
                    shipModel = new SupportShipModel(Id, Length, Speed, RadiusAction, Direction, ((SupportShipViewModel)this).Health);
                    break;
                case nameof(HybridShipViewModel):
                    shipModel = new HybridShipModel(Id, Length, Speed, RadiusAction, Direction, ((HybridShipViewModel)this).Damage, ((HybridShipViewModel)this).Health);
                    break;
            }

            return shipModel;

        }

        public ShipViewModel() { }
    }
}
