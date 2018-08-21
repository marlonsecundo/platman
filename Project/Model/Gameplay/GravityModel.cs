using Microsoft.Xna.Framework;

namespace Model.Gameplay
{
    public class GravityModel : BlockModel
    {
        public string orientation;
        public GravityModel()
        {

        }

        public GravityModel(string texture, Vector2 position, Rectangle bounds, bool visible, string orientation) : base(texture, position, bounds, visible)
        {
            this.orientation = orientation;
        }
    }
}
