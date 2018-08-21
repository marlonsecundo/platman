using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Platman.Component.Base;
using Platman.Component.GameScreen.Interface.Button;
using Platman.Component.GameScreen.Screens.Interface.Button;
using Platman.Component.Managers;
using System.Collections.Generic;

namespace Platman.Component.Input
{
    public class InputScreen : BaseScreen, IInput
    {
        private static InputScreen _instance;
        public static InputScreen Instance => _instance = _instance ?? new InputScreen();
        public bool IsConnected { get; private set; }
        public GameKey[] PressedKeys { get; private set; }

        private InputScreen() : base(ScreenContent.input)
        {
            DrawOrder = (int)GameDrawOrder.Input;

            switch(Device.Instance.DeviceType)
            {
                case DType.Android:
                case DType.IOS:
                case DType.Win10_Phone:
                    IsConnected = true;
                    ScreenManager.Instance.ShowInputScreen(this);
                    break;
                default:
                    Enabled = false;
                    Visible = false;
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            PressedKeys = GetPressedKeys();
        }

        protected override GameButton[] LoadButton()
        {
            ButtonInput[] buttons = new ButtonInput[Model.buttons.Length];

            for (int i = 0; i < buttons.Length; i++)
                buttons[i] = new ButtonInput(Model.buttons[i], this, (object sender) => { });

            return buttons;
        }

        private GameKey[] GetPressedKeys()
        {
            List<GameKey> keys = new List<GameKey>();

            for (int i = 0; i < Buttons.Length; i++)
            {
                if (Buttons[i].State == ButtonState.Pressed)
                    keys.Add(Buttons[i].Key);
            }

            return keys.ToArray();
        }

        public void ClearInput()
        {
            for (int i = 0; i < Buttons.Length; i++)
                Buttons[i].State = ButtonState.Released;
        }
    }
}
