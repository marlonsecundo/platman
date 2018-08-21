using Microsoft.Xna.Framework;
using Model.Base;

namespace Model.Screen
{
    public class ButtonModel : CompModel
    {
        public string name;
        public int delay;
        public string key;

        public ButtonModel()
        {

        }

        public ButtonModel(string key, int delay, string name, Vector2 position) : base(position)
        {
            this.delay = delay;
            this.name = name;
            this.key = key;
        }

    }
}
