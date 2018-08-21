using Microsoft.Xna.Framework;
using Platman.Component.Managers;
using Platman.Extensions;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Utilities;

namespace Platman.Component.Gameplay
{
    public class WorldComponent : AnimatedComponent
    {
        public override Vector2 Position { get => Body.Position; set => Body.Position = value; }
        public Body Body { get;  set; }
        protected World World { get; }
        public Vector2 DrawPosition => ConvertUnits.ToDisplayUnits(Body.Position);
        public override Rectangle DrawBounds => new Rectangle((int)DrawPosition.X, (int)DrawPosition.Y, base.DrawBounds.Width, base.DrawBounds.Height);
        public Vector2 DrawOrigin => new Vector2(DrawBounds.Width / 2f, DrawBounds.Height / 2f);

        public virtual RectangleFloat Bounds
        {
            get
            {
                Vector2 value = ConvertUnits.ToSimUnits(DrawWidth, DrawHeight);
                return new RectangleFloat(Position, value);
            }
        }

        public WorldComponent(World world, AnimationManager animationManager, int drawOrder = 0, Camera2D camera = null) : base(animationManager, drawOrder, camera)
        {
            World = world;
        }
    }
}
