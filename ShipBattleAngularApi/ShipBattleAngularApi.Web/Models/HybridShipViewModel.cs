namespace ShipBattleAngularApi.Web.Models
{
    public class HybridShipViewModel : ShipViewModel
    {
        public HybridShipViewModel(int id, int length, int speed, int radiusAction, int direction, int damage, int health) : base(id, length, speed, radiusAction, direction)
        {
            Damage = damage;
            Health = health;
        }
        public HybridShipViewModel(int length, int speed, int radiusAction, int damage, int health) : base(length, speed, radiusAction)
        {
            Damage = damage;
            Health = health;
        }

        public HybridShipViewModel()
        {
        }

        public int Damage { get; set; }
        public int Health { get; set; }
    }
}
