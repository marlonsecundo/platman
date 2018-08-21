using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Model.Screen;
using Platman.Component.Managers;

namespace Platman.Component.GameScreen.Screens.Interface
{
    public class Art : AnimatedComponent
    {
        public Art(ArtModel model) : base(model, new AnimationManager(model.animations))
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            CurrentAnimation.NextFrame(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);


            // Draw call

            spriteBatch.Begin(transformMatrix: Camera.Matrix);

            spriteBatch.Draw(Texture, Position, Frame, Color.White * Alpha, 0, Vector2.Zero, 1f, HorizontalSide, 0);

            spriteBatch.End();



        }
    }
}
