namespace Platman.Component.Input
{
    public class HeroKeys
    {
        private HeroKeys()
        {

        }

        public readonly GameKey[] Left = new GameKey[] { GameKey.Left };
        public readonly GameKey[] Right = new GameKey[] { GameKey.Right };
        public readonly GameKey[] Jump = new GameKey[] { GameKey.A };
        public readonly GameKey[] Down = new GameKey[] { GameKey.Down };
        public readonly GameKey[] DownLeft = new GameKey[] { GameKey.Down, GameKey.Left };
        public readonly GameKey[] DownRight = new GameKey[] { GameKey.Down, GameKey.Right };
        public readonly GameKey[] Gravity = new GameKey[] { GameKey.B };
        private static HeroKeys _instance;

        public static HeroKeys Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new HeroKeys();

                return _instance;
            }
        }
    }
}
