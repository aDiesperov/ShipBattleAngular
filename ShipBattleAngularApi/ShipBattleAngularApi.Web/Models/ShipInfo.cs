namespace ShipBattleAngularApi.Web.Models
{
    public class ShipInfo
    {
        public ShipInfo(string typeShip, int x, int y, int dir, int len, int speed, int r_act, int act1, int act2 = 0)
        {
            TypeShip = typeShip;
            this.x = x;
            this.y = y;
            this.dir = dir;
            this.len = len;
            this.speed = speed;
            this.r_act = r_act;
            this.act1 = act1;
            this.act2 = act2;
        }

        public ShipInfo()
        {

        }

        public string TypeShip { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int dir { get; set; }
        public int len { get; set; }
        public int speed { get; set; }
        public int r_act { get; set; }
        public int act1 { get; set; }
        public int act2 { get; set; }
    }
}
