using Microsoft.Xna.Framework;
using Platman.Component.Gameplay.Const;
using Platman.Component.Gameplay.Mechanic.Gravity;
using Platman.Component.Input;
using Platman.Extensions;
using System;
using VelcroPhysics.Dynamics;

namespace Platman.Component.Gameplay.Person
{


    public class MechanicControler
    {
        HeroKeys keys;
        PlayerInput input;

        public GravityManager GravityManager { get; }
        public Vector2 MaxVelocity { get; }
        public Vector2 Acceleration { get; private set; }
        public Vector2 Velocity => GravityManager.RealVelocity(Body.LinearVelocity);
        public Body Body { get; }
        public World World { get; }
        public Hero Platman { get; }
        public bool IsOverGround => isOverGround && !GravityManager.IsRunning;

        WorldComponent overGround;


        bool isOverGround;

        public MechanicControler(Hero hero, World world, GravityManager gravityManager)
        {
            Platman = hero;
            Body = hero.Body;
            World = world;
            MaxVelocity = new Vector2(8f, Math.Abs(VelocityValues.Instance.Down.Y));
            GravityManager = gravityManager;

            Body.OnCollision += Body_OnCollision;
            Body.OnSeparation += Body_OnSeparation;

            input = PlayerInput.Instance;
            keys = HeroKeys.Instance;
        }

        private void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB, VelcroPhysics.Collision.ContactSystem.Contact contact)
        {
            WorldComponent ground = (fixtureB.Body.UserData as WorldComponent);

            if (overGround == ground)
                isOverGround = false;
        }

        private void Body_OnCollision(Fixture fixtureA, Fixture fixtureB, VelcroPhysics.Collision.ContactSystem.Contact contact)
        {
            WorldComponent ground = (fixtureB.Body.UserData as WorldComponent);

            if (ground != null)
            {
                RectangleFloat bounds = ground.Bounds;
                RectangleFloat heroBounds = Platman.Bounds;

                var bottom = Math.Abs(heroBounds.Bottom - bounds.Top);
                var top = Math.Abs(heroBounds.Top - bounds.Bottom);
                var left = Math.Abs(heroBounds.Left - bounds.Right);
                var right = Math.Abs(heroBounds.Right - bounds.Left);

                float[] values = new float[] { bottom, top, left, right };

                Array.Sort(values);

                float edge = values[0];

                bool b = false;

                if (World.Gravity == VelocityValues.Instance.GravityDown)
                {
                    if (heroBounds.Bottom <= bounds.Top && bottom == edge)
                        b = true;
                }
                else if (World.Gravity == VelocityValues.Instance.GravityUp)
                {
                    if (heroBounds.Top > bounds.Bottom && top == edge)
                        b = true;
                }
                else if (World.Gravity == VelocityValues.Instance.GravityRight)
                {
                    if (heroBounds.Right <= bounds.Left && right == edge)
                        b = true;
                }
                else if (World.Gravity == VelocityValues.Instance.GravityLeft)
                {
                    if (heroBounds.Left > bounds.Right && left == edge)
                        b = true;
                }

                if (b)
                {
                    isOverGround = true;
                    overGround = ground;
                    ApplyFallColisionImpulse();
                }
            }
        }

        private void ApplyFallColisionImpulse()
        {
            Vector2 velo = Velocity.Abs();

            Vector2 force = Vector2.Zero;

            force.X = Math.Sign(Velocity.X) * Math.Abs(velo.Y / 2.75f);
            ApplyLinearImpulse(force);
        }

        public void Update(GameTime gameTime)
        {
            Vector2 force = Vector2.Zero;

            HandleInput(ref force);

            Acceleration = new Vector2(force.X / Body.Mass, force.Y / Body.Mass);

            UpdateVelocity();
            
        }

        private void UpdateVelocity()
        {
            if (Math.Abs(Velocity.X) > MaxVelocity.X)
            {
                var value = new Vector2(Math.Sign(Velocity.X) * MaxVelocity.X, Velocity.Y);
                value = GravityManager.ApplyGravity(value);

                value = new Vector2(Math.Abs(value.X), Math.Abs(value.Y));

                value.X *= Math.Sign(Body.LinearVelocity.X);
                value.Y *= Math.Sign(Body.LinearVelocity.Y);

                Body.LinearVelocity = value;
            }

            if (Math.Abs(Velocity.Y) > MaxVelocity.Y)
            {
                var value = new Vector2(Velocity.X, Math.Sign(Velocity.Y) * MaxVelocity.Y);
                value = GravityManager.ApplyGravity(value);
                
                value = new Vector2(Math.Abs(value.X), Math.Abs(value.Y));

                value.X *= Math.Sign(Body.LinearVelocity.X);
                value.Y *= Math.Sign(Body.LinearVelocity.Y);

                Body.LinearVelocity = value;
            }
        }

        private void HandleInput(ref Vector2 force)
        {
            force = Vector2.Zero;

            if (input.IsUnClearKeyDown(GameKey.X) && IsOverGround)
            {
                ChangeGravity(-World.Gravity);
            }

            if (input.IsComboKeyDown(keys.DownRight))
            {
                force = VelocityValues.Instance.Run;
                Slide(force);
            }
            else if (input.IsComboKeyDown(keys.DownLeft))
            {
                force = -VelocityValues.Instance.Run;
                Slide(force);
            }
            else if (input.IsAnyKeyDown(keys.Jump))
            {
                force = VelocityValues.Instance.JumpImpulse;
                Jump(force);
            }
            else if (input.IsAnyKeyDown(keys.Left))
            {
                force = -VelocityValues.Instance.Run;

                var x = Velocity.X;

                if (Math.Sign(force.X) != Math.Sign(x) && Velocity.X != 0)
                {
                    force = VelocityValues.Instance.Stop * Math.Sign(Velocity.X) * -1;
                    Stop(force);
                }
                else
                    Run(force);
            }
            else if (input.IsAnyKeyDown(keys.Right))
            {
                force = VelocityValues.Instance.Run;

                if (Math.Sign(force.X) != Math.Sign(Velocity.X) && Velocity.X != 0)
                {
                    force = VelocityValues.Instance.Stop * Math.Sign(Velocity.X) * -1;
                    Stop(force);

                }
                else
                    Run(force);
            }
            else if (input.IsAnyKeyDown(keys.Down))
            {
                Stop(VelocityValues.Instance.Stop * Math.Sign(Velocity.X) * -1);
            }
        }

        private void Run(Vector2 force)
        {
            if (IsOverGround)
            {
                if (Platman.StateManager.CurrentState != State.Running && Math.Abs(Velocity.X) < 1f)
                {
                    ApplyLinearImpulse(VelocityValues.Instance.RunImpulse * Math.Sign(force.X));
                }
                else if (Platman.StateManager.CurrentState == State.Running)
                {
                    ApplyForce(force);
                }

                Platman.StateManager.ChangeState(State.Running);
            }
            else
            {
                if (Math.Abs(Velocity.X) < 3f)
                    Move(VelocityValues.Instance.Move);
                Platman.StateManager.ChangeState(State.Jumping);
            }
        }

        private void Stop(Vector2 force)
        {
            if (IsOverGround)
            {
                Platman.StateManager.ChangeState(State.Stopping);
                ApplyForce(force);
            }
        }

        private void Jump(Vector2 impulse)
        {
            if (IsOverGround)
            {
                ApplyLinearImpulse(impulse);
                Platman.StateManager.ChangeState(State.Jumping);
            }
        }

        private void Move(Vector2 force)
        {
            ApplyForce(force);
            Platman.StateManager.ChangeState(State.Jumping);
        }

        private void Slide(Vector2 impulse)
        {
            if (IsOverGround && Math.Abs(Body.LinearVelocity.X) > 10)
                Platman.StateManager.ChangeState(State.Sliding);
        }

        public void ChangeGravity(Vector2 value)
        {
            GravityManager.UpdateGravity(value);
            Body.ApplyLinearImpulse(new Vector2(1f * Math.Sign(World.Gravity.X), 1f * Math.Sign(World.Gravity.Y)));
            Platman.StateManager.ChangeState(State.Jumping);
        }

        private void ApplyForce(Vector2 value)
        {
            value = GravityManager.ApplyGravity(value);
            Body.ApplyForce(value);
        }

        private void ApplyLinearImpulse(Vector2 value)
        {
            value = GravityManager.ApplyGravity(value);

            Body.ApplyLinearImpulse(value);
        }
    }
}
