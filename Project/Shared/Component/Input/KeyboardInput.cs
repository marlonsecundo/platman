using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Platman.Component.Input
{
    public class KeyboardInput : GameComponent, IInput
    {
        private static KeyboardInput _instance;
        public static KeyboardInput Instance => _instance = _instance ?? new KeyboardInput();
        public bool IsConnected { get; private set; }
        public GameKey[] PressedKeys { get; private set; }

        public KeyboardInput() : base(GameRoot.Instance)
        {
            switch(Device.Instance.DeviceType)
            {
                case DType.Desktop:
                case DType.Win10_PC:
                    IsConnected = true;
                    ClearInput();
                    Game.Components.Add(this);
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            GetPressedKeys();
        }

        private void GetPressedKeys()
        {
            List<GameKey> gameKeys = new List<GameKey>();

            Keys[] keys = Keyboard.GetState().GetPressedKeys();

            foreach (Keys k in keys)
            {
                switch (k)
                {
                    case Keys.W: case Keys.Up: gameKeys.Add(GameKey.Up); break;
                    case Keys.A: case Keys.Left: gameKeys.Add(GameKey.Left); break;
                    case Keys.S: case Keys.Down: gameKeys.Add(GameKey.Down); break;
                    case Keys.D: case Keys.Right: gameKeys.Add(GameKey.Right); break;
                    case Keys.K: gameKeys.Add(GameKey.B); break;
                    case Keys.Space: gameKeys.Add(GameKey.A); break;
                    case Keys.LeftShift: gameKeys.Add(GameKey.X); break;
                    case Keys.J: gameKeys.Add(GameKey.Y); break;
                    case Keys.Enter: gameKeys.Add(GameKey.Enter); break;
                    case Keys.Back: gameKeys.Add(GameKey.Back); break;
                    case Keys.Tab: gameKeys.Add(GameKey.Tab); break;
                    case Keys.Q: gameKeys.Add(GameKey.RT); break;
                    case Keys.E: gameKeys.Add(GameKey.LT); break;
                    case Keys.Escape: gameKeys.Add(GameKey.Esc); break;
                }
            }

            if (PressedKeys.Length> 0)
            {

            }

            PressedKeys = gameKeys.ToArray();
        }

        public void ClearInput()
        {
            PressedKeys = new GameKey[] { };
        }
    }
}
