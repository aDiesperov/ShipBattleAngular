namespace ShipBattleAngularApi.Web.Models
{
    public class MilitaryShipViewModel : ShipViewModel
    {
        public MilitaryShipViewModel(int id, int length, int speed, int radiusAction, int direction, double damage) : base(id, length, speed, radiusAction, direction)
        {
            Damage = damage;
        }
        public MilitaryShipViewModel(int length, int speed, int radiusAction, double damage) : base(length, speed, radiusAction)
        {
            Damage = damage;
        }

        public MilitaryShipViewModel()
        {
        }

        public double Damage { get; set; }
    }
}
