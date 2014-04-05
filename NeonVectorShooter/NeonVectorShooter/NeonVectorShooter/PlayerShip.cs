namespace NeonVectorShooter
{
    public class PlayerShip : Entity
    {
        private static PlayerShip _instance;
        public static PlayerShip Instance
        {
            get { return _instance ?? (_instance = new PlayerShip()); }
        }

        private PlayerShip()
        {
            Image = Art.Player;
            Position = GameRoot.ScreenSize / 2f;
            Radius = 10;
        }

        public override void Update()
        {

        }
    }
}