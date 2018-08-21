using Microsoft.Xna.Framework;

namespace Model.Gameplay
{
    public class TextPointModel : BlockModel
    {
        public string text;
        public TextPointModel()
        {

        }

        public TextPointModel(string text, string texture, Vector2 position, Rectangle bounds, bool visible) : base(texture, position, bounds, visible)
        {
            this.text = text;
        }
    }
}
