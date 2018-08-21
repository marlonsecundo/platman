using Microsoft.Xna.Framework;

namespace Model.Gameplay
{
    public class PoetryModel : BlockModel
    {
        public string text;

        public PoetryModel()
        {

        }

        public PoetryModel(string text, string texture, Vector2 position, Rectangle bounds, bool visible) : base(texture, position, bounds, visible)
        {
            this.text = text;
        }

    }
}
