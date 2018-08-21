using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Platman.Component.Audio;
using Platman.Component.Effect;
using Platman.Component.Gameplay.Const;
using Platman.Component.Gameplay.Map;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Dynamics.Solver;

namespace Platman.Component.Gameplay.Person
{
    public enum State
    {
        Standy,
        Running,
        Jumping,
        Sliding,
        Stopping,
        Waiting,
        Fall,
        None
    }


    public sealed class StateManager
    {
        RotationEffect rotationEffect;

        public State CurrentState { get; private set; }
        Dictionary<string, SoundEffect> sounds;
        Hero platman;
        World world;

        int time = 0;
        int timeSound = 0;

        public StateManager(Hero hero, World world)
        {
            LoadSounds();
            this.world = world;
            platman = hero;

            rotationEffect = RotationEffect.GetRotationUp(0);
            platman.Mechanic.GravityManager.GravityChanged += GravityManager_GravityChanged;

            CurrentState = State.Standy;

            
        }

        private void GravityManager_GravityChanged(object sender, EventArgs e)
        {
            if (world.Gravity == VelocityValues.Instance.GravityDown)
                rotationEffect = RotationEffect.GetRotationDown(rotationEffect.Angle);
            else if (world.Gravity == VelocityValues.Instance.GravityUp)
                rotationEffect = RotationEffect.GetRotationUp(rotationEffect.Angle);
            else if (world.Gravity == VelocityValues.Instance.GravityLeft)
                rotationEffect = RotationEffect.GetRotationLeft(rotationEffect.Angle);
            else if (world.Gravity == VelocityValues.Instance.GravityRight)
                rotationEffect = RotationEffect.GetRotationRight(rotationEffect.Angle);

            rotationEffect.Enabled = true;
        }

        private void LoadSounds()
        {
            sounds = new Dictionary<string, SoundEffect>();
            sounds.Add(State.Jumping.ToString(), ContentLoader.Instance.LoadSound(State.Jumping.ToString()));
            sounds.Add(State.Running.ToString(), ContentLoader.Instance.LoadSound(State.Running.ToString()));
        }

        public void Update(GameTime gameTime)
        {
            switch (CurrentState)
            {
                case State.Running:

                    if (platman.Mechanic.Velocity.X < 5)
                        platman.CurrentAnimation.Delay = 100;
                    else if (platman.Mechanic.Velocity.X < 6)
                        platman.CurrentAnimation.Delay = 75;
                    else
                        platman.CurrentAnimation.Delay = 50;

                    if (platman.CurrentAnimation.FrameIndex == 4)
                        PlaySoundState(State.Running);

                    if (platman.Mechanic.Acceleration == Vector2.Zero)
                        ChangeState(State.Stopping);

                    break;

                case State.Stopping:

                    if (Math.Round(platman.Mechanic.Velocity.X) == 0)
                        ChangeState(State.Standy);

                    break;

                case State.Standy:

                    time += gameTime.ElapsedGameTime.Milliseconds;

                    if (time > 5000)
                        ChangeState(State.Waiting);

                    break;

                case State.Jumping:

                    if (Math.Round(platman.Mechanic.Velocity.Y) == 0 && platman.Mechanic.IsOverGround)
                    {
                        ChangeState(State.Stopping);
                    }

                    break;
            }

            rotationEffect.RunEffect(gameTime);
            platman.Body.Rotation = rotationEffect.Radian;
        }

        public void ChangeState(State state)
        {
            if (CurrentState != state)
            {
                platman.ChangeAnimation(state);
                CurrentState = state;
                time = 0;
            }
        }

        public void ForceState(State name)
        {
            CurrentState = name;
            time = 0;
            timeSound = 0;
            platman.ChangeAnimation(name);
        }

        private void PlaySoundState(State name)
        {
            if (name == State.Jumping || name == State.Running)
            {
                if (timeSound >= platman.CurrentAnimation.Delay)
                {
                    bool able = false;
                    switch (name)
                    {
                        case State.Jumping:
                            if (platman.CurrentAnimation.Position == Vector2.Zero)
                                able = true;
                            break;

                        case State.Running:
                            if (platman.CurrentAnimation.Position == Vector2.Zero)
                                able = true;
                            break;
                    }

                    if (able)
                    {
                        GameSoundPlayer.Instance.PlaySound(sounds[name.ToString()]);
                        timeSound = 0;
                    }

                }
            }
        }

        private void UpdateEffect(GameTime gameTime)
        {
            rotationEffect.RunEffect(gameTime);
        }
    }
}
