using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Model.Base;
using Platman.Component.Base;
using Platman.Component.Managers;

namespace Platman.Component
{
    public abstract partial class AnimatedComponent : DrawableBase
    {
        public virtual int DrawWidth => animationManager.CurrentAnimation.Frame.Width;
        public virtual int DrawHeight => animationManager.CurrentAnimation.Frame.Height;
        public Texture2D[] AllTextures { get { return animationManager.AllTextures; } }
        public Animation CurrentAnimation { get { return animationManager.CurrentAnimation; } }
        public Texture2D Texture { get { return animationManager.CurrentAnimation.Texture; } }
        public Rectangle Frame { get { return animationManager.CurrentAnimation.Frame; } }
        public override float Alpha { get; set; }
        public SpriteEffects HorizontalSide { get; set; }
        public override Rectangle DrawBounds => new Rectangle((int)Position.X, (int)Position.Y, DrawWidth, DrawHeight);

        private AnimationManager animationManager;

        protected int time;


        public AnimatedComponent(CompModel model, AnimationManager animationManager, int drawOrder = 0, Camera2D camera = null) : base(camera)
        {
            LoadOutherComps();

            Position = model.position;

            _Constructor(drawOrder);

            this.animationManager = animationManager;
            Game.Components.Add(this);
        }

        public AnimatedComponent(AnimationManager animationManager, int drawOrder = 0, Camera2D camera = null) : base(camera)
        {
            _Constructor(drawOrder);
            this.animationManager = animationManager;
            Game.Components.Add(this);
        }

        private void _Constructor(int drawOrder)
        {
            HorizontalSide = SpriteEffects.None;
            Alpha = 1;
            DrawOrder = drawOrder;
        }


        protected virtual void LoadOutherComps()
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            time += gameTime.ElapsedGameTime.Milliseconds;

        }

        public virtual Animation ChangeAnimation(object animationKey)
        {
            return animationManager.ChangeAnimation(animationKey);
        }

        public override void Unload()
        {
            base.Unload();
        }


    }
}
