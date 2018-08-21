using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Platman.Component.Input
{
    public class GamePadInput : GameComponent, IInput
    {
        private static GamePadInput _instance;
        public static GamePadInput Instance => _instance = _instance ?? new GamePadInput();
        public GameKey[] PressedKeys { get; private set; }

        private bool _isConnected;
        public bool IsConnected { get => GamePad.GetCapabilities(PlayerIndex.One).IsConnected; }

        public GamePadInput() : base(GameRoot.Instance)
        {
            ClearInput();
            _isConnected = GamePad.GetCapabilities(PlayerIndex.One).IsConnected;
            Game.Components.Add(this);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            PressedKeys = GetPressedKeys();

            if (_isConnected != IsConnected)
            {
                PlayerInput.Instance.ChangePreferedInput();
                _isConnected = IsConnected;
            }
        }

        private GameKey[] GetPressedKeys()
        {
            List<GameKey> gameKeys = new List<GameKey>();

            GamePadState state = GamePad.GetState(PlayerIndex.One);

            if (state.Buttons.A == ButtonState.Pressed)
                gameKeys.Add(GameKey.A);
            if (state.Buttons.B == ButtonState.Pressed)
                gameKeys.Add(GameKey.B);
            if (state.Buttons.X == ButtonState.Pressed)
                gameKeys.Add(GameKey.X);
            if (state.Buttons.Y == ButtonState.Pressed)
                gameKeys.Add(GameKey.Y);
            if (state.DPad.Left == ButtonState.Pressed)
                gameKeys.Add(GameKey.Left);
            if (state.DPad.Right == ButtonState.Pressed)
                gameKeys.Add(GameKey.Right);
            if (state.DPad.Up == ButtonState.Pressed)
                gameKeys.Add(GameKey.Up);
            if (state.DPad.Down == ButtonState.Pressed)
                gameKeys.Add(GameKey.Down);
            if (state.Buttons.Start == ButtonState.Pressed)
                gameKeys.Add(GameKey.Enter);
            if (state.Buttons.Back == ButtonState.Pressed)
                gameKeys.Add(GameKey.Back);
            if (state.IsButtonDown(Buttons.LeftTrigger))
                gameKeys.Add(GameKey.LT);
            if (state.IsButtonDown(Buttons.RightTrigger))
                gameKeys.Add(GameKey.RT);

            
            return gameKeys.ToArray();
        }

        public void ClearInput()
        {
            PressedKeys = new GameKey[0];
        }

       
    }
}
