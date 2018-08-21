using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Model;
using Model.Gameplay;
using Platman.Component.Base;
using Platman.Component.Gameplay.Mechanic.Collision;
using Platman.Component.Managers;
using VelcroPhysics.Collision.Filtering;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Factories;
using VelcroPhysics.Utilities;

namespace Platman.Component.Gameplay.Map
{

    public class Block : WorldComponent
    {
        private string name;

        public override int DrawWidth { get; }
        public override int DrawHeight { get; }


        public Block(BlockModel model, World world, AnimationManager anim = null) : base(world, anim ?? LoadAnimation(model.texture), (int)GameDrawOrder.Blocks)
        {
            Camera = CamManager.Instance.LevelCamera;
            DrawWidth = model.bounds.Width;
            DrawHeight = model.bounds.Height;
            Visible = model.visible;
            name = model.texture;

            Enabled = false;

            var width = ConvertUnits.ToSimUnits(model.bounds.Width);
            var height = ConvertUnits.ToSimUnits(model.bounds.Height);
            var x = ConvertUnits.ToSimUnits(model.position.X);
            var y = ConvertUnits.ToSimUnits(model.position.Y);

            Body = BodyFactory.CreateRectangle(world, width, height, 1f, new Vector2(x, y), 0f, BodyType.Static);
            Body.UserData = this;
            Body.CollidesWith = Collide.Instance.BlockWith;
            Body.CollisionCategories = Collide.Instance.BlockGroup;

            Body.Awake = false;
        }

        private static AnimationManager LoadAnimation(string texture)
        {
            var model = new AnimationModel("key1", "Textures/Gameplay/Map/" + texture, 1,1,1, 100, false);
            Animation anim = new Animation(model );
            return new AnimationManager(new Animation[] { anim });
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            CurrentAnimation.NextFrame(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Rectangle bounds = new Rectangle(0, 0, DrawBounds.Width, DrawBounds.Height);
            
            spriteBatch.Begin(transformMatrix: CamManager.Instance.LevelCamera.Matrix, sortMode: SpriteSortMode.Immediate, samplerState: SamplerState.LinearWrap);

            spriteBatch.Draw(Texture, DrawPosition, DrawBounds, Color.White * Alpha, 0, bounds.Center.ToVector2(), 1f, SpriteEffects.None, 0);

            spriteBatch.End();
        }
    }
}
