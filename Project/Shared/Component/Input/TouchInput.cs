using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Platman.Component.Managers;

namespace Platman.Component.Input
{
    public class TouchInput : GameComponent, IInput
    {
        private static TouchInput _instance;
        public static TouchInput Intance => _instance = _instance ?? new TouchInput();

        public GameKey[] PressedKeys { get;
            private set; }
        public bool IsConnected { get; }
        public Point[] Position { get; private set; }

        private CamManager camManager;

        public TouchInput() : base(GameRoot.Instance)
        {
            camManager = CamManager.Instance;
            PressedKeys = new GameKey[1];
            switch (Device.Instance.DeviceType)
            {
                case DType.Android:
                case DType.IOS:
                case DType.Win10_Phone:
                    IsConnected = true;
                    Game.Components.Add(this);
                    break;
            }
            ClearInput();
        }

        public void ClearInput()
        {
            PressedKeys[0] = GameKey.TouchLeftReleased;
            Position = new Point[0];
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            TouchCollection state = TouchPanel.GetState();

            if (state.Count > 0)
            {
                Position = new Point[state.Count];
                PressedKeys[0] = GameKey.TouchLeftPressed;

                for (int i = 0; i < Position.Length; i++)
                    Position[i] = camManager.ScaleMouseToScreenCoordinates(state[i].Position).ToPoint();
            }
            else
                PressedKeys[0] = GameKey.TouchLeftReleased;

        }

    }
}
