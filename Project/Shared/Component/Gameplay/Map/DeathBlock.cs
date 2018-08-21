using Model;
using Model.Gameplay;
using Platman.Component.Base;
using Platman.Component.Managers;
using VelcroPhysics.Dynamics;

namespace Platman.Component.Gameplay.Map
{
    public sealed class DeathBlock : Block
    {
        public bool Colided { get; set; }
        public DeathBlock(BlockModel model, World world) : base(model, world, LoadAnimation(model.texture))
        {
            DrawOrder = (int)GameDrawOrder.Blocks;
            Visible = false;
        }

        private static AnimationManager LoadAnimation(string texture)
        {
            var model = new AnimationModel("key1", "Textures/Gameplay/Map/" + texture, 1,1,1, 100, true);
            Animation anim = new Animation(model);
            return new AnimationManager(new Animation[] { anim });
        }
    }
}
