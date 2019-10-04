using EncryptionAttributes.Attributes;
using ShipBattleApi.Models.Models;
using System.Collections.Generic;

namespace ShipBattleAngularApi.Web.Models
{
    public class GameFieldViewModel
    {
        public GameFieldViewModel(GameFieldModel gameFieldModel)
        {
            Id = gameFieldModel.Id;
            Name = gameFieldModel.Name;
            Coors = gameFieldModel.Coors;
        }
        public GameFieldViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public GameFieldViewModel(int id)
        {
            Id = id;
        }

        public GameFieldViewModel() { }
        [Encryption("EncryptionAttributes.Services.EncryptionService", "P@@Sw0rd", "S@LT&KEY", "@1B2c3D4e5F6g7H8")]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CoorModel> Coors { get; set; } = new List<CoorModel>();

        internal GameFieldModel MapToGameFieldModel()
        {
            return new GameFieldModel(Id, Name);
        }
    }
}
