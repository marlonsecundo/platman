using Microsoft.Xna.Framework;
using Platman.Component.Base;
using System;

namespace Platman.Component.Effect
{
    public class DelayEffect : BaseEffect
    {
        public DelayEffect(int delay) : base(delay, true, false)
        {
            Enabled = true;
        }

        public override event EventHandler Finish;

        public override void RunEffect(GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime.Milliseconds;

            if (Enabled)
            {
                if (time > Delay)
                {
                    Finish(this, null);
                    Enabled = false;
                }
            }
        }
    }
}
