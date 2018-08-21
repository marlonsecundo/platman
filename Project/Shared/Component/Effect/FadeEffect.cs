using Microsoft.Xna.Framework;
using Platman.Component.Base;
using System;

namespace Platman.Component.Effect
{
    public sealed class FadeEffect : BaseEffect
    {

        private bool _isFocus;
        public bool IsFocus
        {
            get { return _isFocus; }
            set
            {
                if (_isFocus != value)
                    Enabled = true;
                _isFocus = value;
            }
        }

        private float alpha;
        public float Alpha
        {
            get { return alpha; }
            set
            {

                alpha = value;


                if (alpha > MinAlpha && !IsFocus)
                    alpha = MinAlpha;

            }
        }
        public float MaxAlpha { get; private set; }
        public float MinAlpha { get; private set; }

        public override event EventHandler Finish;

        public FadeEffect(bool isFocused, int delay, float maxAlpha, float minAlpha, bool await = false, bool repeat = false) : base(delay, await, repeat)
        {
            IsFocus = isFocused;

            MaxAlpha = maxAlpha;
            MinAlpha = minAlpha;

            if (IsFocus)
                alpha = maxAlpha;
            else
                alpha = minAlpha;

            Enabled = false;

            Finish += FadeEffect_Completed;
        }

        private void FadeEffect_Completed(object sender, EventArgs e)
        {
            Enabled = false;
        }

        public override void RunEffect(GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime.Milliseconds;

            if (time >= Delay)
            {
                if (Enabled)
                {
                    if (IsFocus)
                        alpha += 0.1f;
                    else
                        alpha -= 0.1f;

                    if (alpha > MaxAlpha)
                    {
                        alpha = MaxAlpha;
                        Finish(this, null);
                    }
                    else if (alpha < MinAlpha)
                    {
                        alpha = MinAlpha;
                        Finish(this, null);
                    }

                }

                time = 0;
            }
        }
    }
}
