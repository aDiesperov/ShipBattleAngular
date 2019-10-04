namespace ShipBattleAngularApi.Web.Models
{
    public class HybridShipViewModel : ShipViewModel
    {
        public HybridShipViewModel(int id, int length, int speed, int radiusAction, int direction, double damage, double health) : base(id, length, speed, radiusAction, direction)
        {
            Damage = damage;
            Health = health;
        }
        public HybridShipViewModel(int length, int speed, int radiusAction, double damage, double health) : base(length, speed, radiusAction)
        {
            Damage = damage;
            Health = health;
        }

        public HybridShipViewModel()
        {
        }

        public double Damage { get; set; }
        public double Health { get; set; }
    }
}
