using Microsoft.Xna.Framework;
using Model.Base;

namespace Model.Screen
{
    public class ScreenModel
    {
        public string path;
        public ArtModel[] arts;
        public ButtonModel[] buttons;
        public TextModel[] texts;
        public CompModel[] comps;
        public Rectangle bounds;

        public ScreenModel(string path, ArtModel[] arts, ButtonModel[] buttons, TextModel[] texts, CompModel[] comps, Rectangle bounds)
        {
            this.arts = arts;
            this.buttons = buttons;
            this.texts = texts;
            this.comps = comps;
            this.path = path;
            this.bounds = bounds;
        }

        public ScreenModel()
        {

        }
    }
}
