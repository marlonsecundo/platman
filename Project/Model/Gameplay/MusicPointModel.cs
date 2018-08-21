using Microsoft.Xna.Framework;

namespace Model.Gameplay
{
    public class MusicPointModel : BlockModel
    {
        public string music;

        public MusicPointModel()
        {

        }

        public MusicPointModel(string music, string texture, Vector2 position, Rectangle bounds, bool visible) : base(texture, position, bounds, visible)
        {

        }
    }
}
