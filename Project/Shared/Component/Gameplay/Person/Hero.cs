using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Model;
using Model.Base;
using Platman.Component.Base;
using Platman.Component.Gameplay.Mechanic.Collision;
using Platman.Component.Gameplay.Mechanic.Gravity;
using Platman.Component.Managers;
using Platman.Extensions;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Utilities;

namespace Platman.Component.Gameplay.Person
{
    public sealed partial class Hero : WorldComponent
    {
        public StateManager StateManager { get; }
        public MechanicControler Mechanic { get; }
        public override RectangleFloat Bounds => Mechanic.GravityManager.Gravity.Y != 0 ? base.Bounds : base.Bounds.TransformRectangle(Angle.Horizontal);

        public Hero(CompModel model, World world, GravityManager gravityManager) : base(world, LoadAnimation(), (int)GameDrawOrder.Person)
        {
            var width = ConvertUnits.ToSimUnits(Frame.Width);
            var height = ConvertUnits.ToSimUnits(Frame.Height);
            var x = ConvertUnits.ToSimUnits(model.position.X);
            var y = ConvertUnits.ToSimUnits(model.position.Y);

            Body = BodyFactory.CreateRectangle(world, width, height, 1f, new Vector2(x, y), 0f, BodyType.Dynamic);
            Body.Mass = 1f;
            Body.UserData = this;
            Body.FixedRotation = true;

            Camera = CamManager.Instance.LevelCamera;

            Mechanic = new MechanicControler(this, World, gravityManager);
            StateManager = new StateManager(this, world);

            Body.CollidesWith = Collide.Instance.HeroWith;
            Body.CollisionCategories = Collide.Instance.HeroGroup;
          
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateAction(gameTime);
            
            UpdateFrame(gameTime);

            UpdateEffect(gameTime);
        }

        private void UpdateEffect(GameTime gameTime)
        {

        }

        private void UpdateFrame(GameTime gameTime)
        {
            StateManager.Update(gameTime);
            CurrentAnimation.NextFrame(gameTime);
            
            if (World.Gravity.Y > 0 || World.Gravity.X > 0)
            {
                if (Mechanic.Velocity.X > 0) HorizontalSide = SpriteEffects.None;
                else if (Mechanic.Velocity.X < 0)
                    HorizontalSide = SpriteEffects.FlipHorizontally;
            }
            else if (World.Gravity.Y < 0 )
            {
                if (Mechanic.Velocity.X > 0)
                    HorizontalSide = SpriteEffects.FlipHorizontally;
                else if (Mechanic.Velocity.X < 0)
                    HorizontalSide = SpriteEffects.None;
            }
            else if ( World.Gravity.X < 0)
            {
                if (Mechanic.Velocity.X > 0)
                    HorizontalSide = SpriteEffects.FlipHorizontally;
                else if (Mechanic.Velocity.X < 0)
                    HorizontalSide = SpriteEffects.None;
            }
        }

        private void UpdateAction(GameTime gameTime)
        {
            Mechanic.Update(gameTime);
        }       
    }




    public sealed partial class Hero : WorldComponent
    {
        private static AnimationManager LoadAnimation()
        {
            string path = "Textures/Gameplay/Entity/Hero/";
            Animation[] animation = new Animation[7];
            animation[0] = new Animation(new AnimationModel(State.Standy, path + State.Standy, 15, 3, 5, 100, true));
            animation[1] = new Animation(new AnimationModel(State.Running, path + State.Running, 5, 2, 3, 100, true));
            animation[2] = new Animation(new AnimationModel(State.Jumping, path + State.Jumping, 12, 3, 4, 100, false));
            animation[3] = new Animation(new AnimationModel(State.Sliding, path + State.Sliding, 9, 3, 3, 100, true));
            animation[4] = new Animation(new AnimationModel(State.Stopping, path + State.Stopping, 4, 2, 2, 100, true));
            animation[5] = new Animation(new AnimationModel(State.Waiting, path + State.Waiting, 18, 3, 6, 100, true));
            animation[6] = new Animation(new AnimationModel(State.Fall, path + State.Jumping, 12, 3, 4, 100, false));

            AnimationManager anim = new AnimationManager(animation);

            return anim;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin(transformMatrix: Camera.Matrix);

            spriteBatch.Draw(Texture, DrawPosition, Frame, Color.White * Alpha, Body.Rotation, CurrentAnimation.CenterPosition, 1f, HorizontalSide, 0);

            spriteBatch.End();
        }

    }
}
