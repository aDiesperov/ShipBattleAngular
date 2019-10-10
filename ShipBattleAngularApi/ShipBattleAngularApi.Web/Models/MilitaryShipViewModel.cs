namespace ShipBattleAngularApi.Web.Models
{
    public class MilitaryShipViewModel : ShipViewModel
    {
        public MilitaryShipViewModel(int id, int length, int speed, int radiusAction, int direction, int damage) : base(id, length, speed, radiusAction, direction)
        {
            Damage = damage;
        }
        public MilitaryShipViewModel(int length, int speed, int radiusAction, int damage) : base(length, speed, radiusAction)
        {
            Damage = damage;
        }

        public MilitaryShipViewModel()
        {
        }

        public int Damage { get; set; }
    }
}
