using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Model;
using Model.Gameplay;
using Platman.Component.Base;
using Platman.Component.Effect;
using Platman.Component.Managers;
using VelcroPhysics.Dynamics;

namespace Platman.Component.Gameplay.Map
{
    public class PoetryPoint : Block
    {
        public event Handler Collapsed;
        public string Text { get; }

        private FadeEffect fadeEffect;
        private Level level;
        public PoetryPoint(PoetryModel model, World world) : base(model, world, LoadAnimation(model.texture))
        {
            Text = model.text;
            Enabled = true;
            Collapsed += PoetryPoint_Collapsed;
            fadeEffect = new FadeEffect(true, 75, 1f, 0);
            fadeEffect.Finish += FadeEffect_Finish;
        }

        private void FadeEffect_Finish(object sender, System.EventArgs e)
        {
            Collapsed(this);
            Visible = false;
            Enabled = false;
        }

        private void PoetryPoint_Collapsed(object sender)
        {
            Poetry poetry = new Poetry(Text, level);
        }

        private static AnimationManager LoadAnimation(string texture)
        {
            var model = new AnimationModel("key1", "Textures/Gameplay/Map/" + texture, 8, 3, 3, 100, true);
            Animation anim = new Animation(model);
            return new AnimationManager(new Animation[] { anim });
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            fadeEffect.RunEffect(gameTime);
            Alpha = fadeEffect.Alpha;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(transformMatrix: CamManager.Instance.LevelCamera.Matrix);
            spriteBatch.Draw(Texture, Position, Frame, Color.White * Alpha, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            spriteBatch.End();
        }

        public void ShowPoetry(Level level)
        {
            this.level = level;
            level.Enabled = false;
            fadeEffect.IsFocus = false;
            Enabled = true;
        }
    }
}
