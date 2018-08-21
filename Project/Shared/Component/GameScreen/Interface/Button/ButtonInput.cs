
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Model.Screen;
using Platman.Component.Base;
using Platman.Component.GameScreen.Screens.Interface;
using Platman.Component.GameScreen.Screens.Interface.Button;

namespace Platman.Component.GameScreen.Interface.Button
{
    public class ButtonInput : GameButton
    {
        public ButtonInput(ButtonModel model, BaseScreen parent, Handler method, Art art = null, GameText text = null) : base(model, parent, method, art, text)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (State == ButtonState.Pressed)
            {
                time += gameTime.ElapsedGameTime.Milliseconds;
                if (time > (int)Delay)
                {
                    time = 0;
                    InvokeOnClick(this);
                    InvokeOnRelease(this);
                }
            }

            if (State != ButtonState.Pressed && input.IsMouseClick(DrawBounds))
            {
                InvokeOnPressed(this);
            }

        }
    }
}
