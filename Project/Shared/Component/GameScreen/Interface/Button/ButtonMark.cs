using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Model.Screen;
using Platman.Component.Base;

namespace Platman.Component.GameScreen.Screens.Interface.Button
{
    public class ButtonMark : GameButton
    {
        private bool isActivated;
        public ButtonMark(Handler method, BaseScreen parent, ButtonModel model, Art art = null, GameText text = null) : base(model, parent, method, art, text)
        {

        }

        public override void Update(GameTime gameTime)
        {

            if (isActivated)
            {
                time += gameTime.ElapsedGameTime.Milliseconds;
                if (time > (int)Delay)
                {
                    isActivated = false;
                    time = 0;
                }
            }

            if (!isActivated && ((input.IsMouseClick(DrawBounds)) || input.IsKeyDown(Key)))
            {
                switch (State)
                {
                    case ButtonState.Pressed:
                        State = ButtonState.Released;
                        InvokeOnClick(this);
                        InvokeOnRelease(this);
                        break;
                    case ButtonState.Released:
                        State = ButtonState.Pressed;
                        InvokeOnClick(this);
                        InvokeOnPressed(this);
                        break;
                }

                isActivated = true;
            }
        }
    }
}
