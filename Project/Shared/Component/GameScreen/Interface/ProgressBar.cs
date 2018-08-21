using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Model;
using Model.Base;
using Platman.Component.Managers;

namespace Platman.Component.GameScreen.Screens.Interface
{
    public class ProgressBar : AnimatedComponent
    {
        private int _value;
        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                if (_value > 10) _value = 10;
                else if (_value < 0) _value = 0;

                CurrentAnimation.JumpToFrame(_value + 1);
            }
        }

        public override float Alpha
        {
            get => base.Alpha;
            set => base.Alpha = value;
        }
        public ProgressBar(int value, CompModel model) : base(model, new AnimationManager(new AnimationModel("key", "Textures/Screen/Settings/bar", 11, 6, 2, 100, false)))
        {
            Value = value;
        }


        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin(transformMatrix: Camera.Matrix);

            spriteBatch.Draw(Texture, Position, Frame, Color.White * Alpha, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
    }
}
