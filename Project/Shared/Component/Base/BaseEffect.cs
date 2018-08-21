using Microsoft.Xna.Framework;
using System;

namespace Platman.Component.Base
{
    public abstract class BaseEffect
    {
        public abstract event EventHandler Finish;
        public int Delay { get; }
        public bool Await { get; }
        public bool Repeat { get; }
        public bool Enabled { get; set; }

        protected int time;
        public BaseEffect(int delay, bool await, bool repeat)
        {
            Delay = delay;
            Await = await;
            Repeat = repeat;
        }

        public abstract void RunEffect(GameTime gameTime);
    }
}
