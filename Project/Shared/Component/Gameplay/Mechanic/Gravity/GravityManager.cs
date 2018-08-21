using Microsoft.Xna.Framework;
using Platman.Component.Input;
using System;
using VelcroPhysics.Dynamics;

namespace Platman.Component.Gameplay.Mechanic.Gravity
{
    public sealed class GravityManager : GameComponent
    {
        public event EventHandler GravityChanged;

        World World;
        public bool GravityBlocked { get; set; }
        public bool IsRunning { get; private set; }
        public Vector2 Gravity { get; private set; }

        int time;

        public GravityManager(World world) : base(GameRoot.Instance)
        {
            GravityChanged += GravityManager_GravityChanged;
            World = world;
            Game.Components.Add(this);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsRunning)
            {
                time += gameTime.ElapsedGameTime.Milliseconds;

                if (time > 100)
                {
                    IsRunning = false;
                    time = 0;
                }
            }
        }

        public void UpdateGravity(Vector2 gravity)
        {
            if (!GravityBlocked && !IsRunning)
            {
                IsRunning = true;
                World.Gravity = gravity;
                Gravity = World.Gravity;
                GravityChanged(this, null);
            }
        }
       

        public Vector2 ApplyGravity(Vector2 value)
        {
            value.X = (float) Math.Round(value.X, 6);
            value.Y = (float)Math.Round(value.Y, 6);

            if (value != Vector2.Zero)
            {
                if (World.Gravity.Y < 0)
                {
                    value = new Vector2(value.X, -value.Y);
                }
                else if (World.Gravity.X < 0)
                {
                    value = new Vector2(-value.Y, -value.X);
                }
                else if (World.Gravity.X > 0)
                {
                    value = new Vector2(value.Y, -value.X);
                }
            }
            return value;
        }

        public Vector2 RealVelocity(Vector2 value)
        {
            value.X = (float)Math.Round(value.X, 6);
            value.Y = (float)Math.Round(value.Y, 6);

            if (value != Vector2.Zero)
            {
                if (World.Gravity.Y != 0)
                {
                    value = new Vector2(value.X, value.Y);
                }
                else if (World.Gravity.X < 0)
                {
                    value = new Vector2(-value.Y, value.X);
                }
                else if (World.Gravity.X > 0)
                {
                    value = new Vector2(-value.Y, value.X);
                }
            }
            return value;
        }

        private void GravityManager_GravityChanged(object sender, EventArgs e)
        {

        }
    }
}
