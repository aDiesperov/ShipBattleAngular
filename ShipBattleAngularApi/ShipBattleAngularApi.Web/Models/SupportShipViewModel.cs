namespace ShipBattleAngularApi.Web.Models
{
    public class SupportShipViewModel : ShipViewModel
    {
        public SupportShipViewModel(int id, int length, int speed, int radiusAction, int direction, double health) : base(id, length, speed, radiusAction, direction)
        {
            Health = health;
        }
        public SupportShipViewModel(int length, int speed, int radiusAction, double health) : base(length, speed, radiusAction)
        {
            Health = health;
        }

        public SupportShipViewModel()
        {
        }

        public double Health { get; set; }
    }
}
