using Microsoft.Xna.Framework;
using Platman.Component.Base;
using System;

namespace Platman.Component.Effect
{
    public class TranslationEffect : BaseEffect
    {
        private readonly Vector2 _velocity;
        private readonly Vector2 _acceleration;

        public override event EventHandler Finish;
        public Vector2 Acceleration { get; private set; }
        public Vector2 Velocity { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 StartPosition { get; }
        public Vector2 FinalPosition { get; }
        public TranslationEffect(Vector2 startPosition, Vector2 finalPosition, Vector2 velocity, Vector2 acceleration, bool await = false, bool repeat = false) : base(0, await, repeat)
        {
            Finish += TranslationEffect_Finish;

            _velocity = velocity;

            if ((finalPosition.Y < startPosition.Y))
                _velocity.Y *= -1;
            if ((finalPosition.X < startPosition.X))
                _velocity.X *= -1;

            _acceleration = acceleration;

            Acceleration = acceleration;
            Velocity = _velocity;


            StartPosition = new Vector2((float)Math.Round(startPosition.X), (float)Math.Round(startPosition.Y));

            FinalPosition = new Vector2((float)Math.Round(finalPosition.X), (float)Math.Round(finalPosition.Y));

            Enabled = true;

            Position = startPosition;
        }

        private void TranslationEffect_Finish(object sender, EventArgs e)
        {

        }

        public override void RunEffect(GameTime gameTime)
        {
            if (Enabled)
            {
                Velocity += Acceleration / gameTime.ElapsedGameTime.Milliseconds;
                Position += Velocity / gameTime.ElapsedGameTime.Milliseconds;

                if (Velocity.X > 0)
                {
                    if (Math.Round(Position.X) >= FinalPosition.X)
                    {
                        Velocity = new Vector2(0, Velocity.Y);
                        Acceleration = new Vector2(0, Acceleration.Y);
                    }
                }
                else if (Velocity.X < 0)
                {
                    if (Math.Round(Position.X) <= FinalPosition.X)
                    {
                        Velocity = new Vector2(0, Velocity.Y);
                        Acceleration = new Vector2(0, Acceleration.Y);
                    }
                }

                if (Velocity.Y > 0)
                {
                    if (Math.Round(Position.Y) >= FinalPosition.Y)
                    {
                        Velocity = new Vector2(Velocity.X, 0);
                        Acceleration = new Vector2(Acceleration.X, 0);
                    }
                }
                else if (Velocity.Y < 0)
                {
                    if (Math.Round(Position.Y) <= FinalPosition.Y)
                    {
                        Velocity = new Vector2(Velocity.X, 0);
                        Acceleration = new Vector2(Acceleration.X, 0);
                    }
                }



                if (Velocity == Vector2.Zero)
                {
                    if (!Repeat)
                    {
                        Enabled = false;
                        Finish(this, null);
                    }
                    else
                    {
                        Position = StartPosition;
                        Velocity = _velocity;
                        Acceleration = _acceleration;
                    }
                }


            }
        }
    }
}
