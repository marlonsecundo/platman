using Microsoft.Xna.Framework;

namespace Model.Gameplay
{
    public class AnimatedBlockModel : BlockModel
    {

        public AnimatedBlockModel()
        {

        }

        public AnimatedBlockModel(string texture, Vector2 position, Rectangle bounds, bool visible) : base(texture, position, bounds, visible)
        {
        }
    }
}
