using Microsoft.Xna.Framework;
using Model.Base;

namespace Model.Screen
{
    public class TextModel : CompModel
    {
        public string size;
        public string text;

        public TextModel()
        {

        }

        public TextModel(Vector2 position, string text, string size) : base(position)
        {
            this.text = text;
            this.size = size;
        }
    }
}
