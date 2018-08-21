using Microsoft.Xna.Framework;
using Model;
using Platman.Component.Base;
using System;

namespace Platman.Component.Effect
{
    public class AnimationEffect : BaseEffect
    {
        public override event EventHandler Finish;
        public Animation Animation { get; }

        public AnimationEffect(AnimationModel model, bool await = false) : base(model.delay, await, model.repeat)
        {
            Animation = new Animation(model);
            Enabled = true;
        }

        public override void RunEffect(GameTime gameTime)
        {
            if (Enabled)
            {
                Animation.NextFrame(gameTime);

                if (Animation.Completed)
                {
                    Finish(this, null);
                    Enabled = false;
                }
            }
        }
    }
}
