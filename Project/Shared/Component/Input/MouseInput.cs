using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Platman.Component.Base;
using Platman.Component.Managers;

namespace Platman.Component.Input
{
    public class MouseInput : DrawableBase, IInput
    {
        private static MouseInput _instance;
        public static MouseInput Instance => _instance = _instance ?? new MouseInput();

        private Texture2D texture;
        private bool enabled;
        private int time;

        private Point previousPosition;

        public bool IsConnected { get; private set; }
        public GameKey[] PressedKeys { get; private set; }
        public new Point Position { get; private set; }
        private MouseInput() : base(CamManager.Instance.ScreenCamera)
        {
            DrawOrder = (int)GameDrawOrder.Input + 99;
            texture = Game.Content.Load<Texture2D>("Textures/Input/mouse");
            PressedKeys = new GameKey[2];

            switch(Device.Instance.DeviceType)
            {
                case DType.Desktop:
                case DType.Win10_PC:
                    IsConnected = true;
                    Game.Components.Add(this);
                    break;
            }

            ClearInput();
        }

        public override void Update(GameTime gameTime)
        {
            Point value = CamManager.Instance.ScaleMouseToScreenCoordinates(Mouse.GetState().Position.ToVector2()).ToPoint();

            if (enabled)
            {
                previousPosition = Position;
                Position = value;

                var state = Mouse.GetState();
       
                PressedKeys[0] = state.LeftButton == ButtonState.Pressed ? GameKey.TouchLeftPressed : GameKey.TouchLeftReleased;
                PressedKeys[1] = state.RightButton == ButtonState.Pressed ? GameKey.TouchRightPressed : GameKey.TouchRightReleased;
            }


            if (previousPosition == value)
            {
                time += gameTime.ElapsedGameTime.Milliseconds;
                if (time > 5000 && enabled)
                    Disable();
            }
            else if (value != previousPosition)
            {
                time = 0;
                Enable();
            }
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(transformMatrix: Camera.Matrix);
            spriteBatch.Draw(texture, Position.ToVector2(), texture.Bounds, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.End();
        }

        public void Disable()
        {
            enabled = false;
            Visible = false;
            ClearInput();
        }

        public void Enable()
        {
            enabled = true;
            Visible = true;
        }

        public void ClearInput()
        {
            PressedKeys[0] = GameKey.TouchLeftReleased;
            PressedKeys[1] = GameKey.TouchRightReleased;
            Position = Point.Zero;
        }


    }
}
