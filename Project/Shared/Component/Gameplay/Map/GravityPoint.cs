using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Model;
using Model.Gameplay;
using Platman.Component.Managers;
using VelcroPhysics.Dynamics;
using Platman.Component.Gameplay.Const;
using Platman.Component.Gameplay.Mechanic.Collision;
using System;
using Platman.Component.Gameplay.Person;

namespace Platman.Component.Gameplay.Map
{
    public class GravityPoint : Block
    {
        private Vector2 gravity;
        private bool Active { get; set; }

        Hero platman;
        public GravityPoint(GravityModel model, Level level) : base(model, level.World, LoadAnimation(model.texture))
        {
            switch (model.orientation)
            {
                case "Down":
                    gravity = VelocityValues.Instance.GravityDown;
                    break;
                case "Up":
                    gravity = VelocityValues.Instance.GravityUp;
                    break;
                case "Left":
                    gravity = VelocityValues.Instance.GravityLeft;
                    break;
                case "Right":
                    gravity = VelocityValues.Instance.GravityRight;
                    break;
            }

            Enabled = true;
            Body.Awake = false;
            Body.IgnoreCCD = true;
            Body.CollisionCategories = Collide.Instance.None;
            Body.CollidesWith = Collide.Instance.None;

            platman = level.Platman;
        }
        public override void Update(GameTime gameTime)
        {
            CurrentAnimation.NextFrame(gameTime);

            bool collided = CheckColision();

            Body.CollisionCategories = Collide.Instance.HeroGroup;

            if (Active && collided)
            {
                Active = false;
                Alpha = 0.5f;
                ChangeGravity();
            }
            else if (!Active)
            {
                time += gameTime.ElapsedGameTime.Milliseconds;

                if (time >= 500)
                {
                    time = 0;
                    Active = true;
                    Alpha = 1f;
                }
            }
        }

        private void ChangeGravity()
        {
            platman.Mechanic.ChangeGravity(gravity);
        }

        private bool CheckColision()
        { 
            if (Bounds.Contains(platman.Bounds))
                return true;

            return false;
        }

        private static AnimationManager LoadAnimation(string texture)
        {
            var model = new AnimationModel("key1", "Textures/Gameplay/Map/" + texture, 16,4,4, 100, true);
            Animation anim = new Animation(model);
            return new AnimationManager(new Animation[] { anim });
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(transformMatrix: Camera.Matrix);

            spriteBatch.Draw(Texture, DrawPosition, Frame, Color.White * Alpha, 0, DrawOrigin, Vector2.One, SpriteEffects.None, 0);

            spriteBatch.End();
        }
    }
}
