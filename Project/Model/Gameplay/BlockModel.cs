using Microsoft.Xna.Framework;
using Model.Base;

namespace Model.Gameplay
{
    public class BlockModel : CompModel
    {
        public string texture;
        public bool visible;
        public Rectangle bounds;

        public BlockModel(string texture, Vector2 position, Rectangle bounds, bool visible) : base(position)
        {
            this.bounds = bounds;
            this.texture = texture;
            this.visible = visible;
        }

        public BlockModel()
        {

        }
    }
}
