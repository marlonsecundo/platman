using Microsoft.Xna.Framework;
using Model.Base;

namespace Model.Screen
{
    public class ArtModel : CompModel
    {
        public AnimationModel[] animations;
        public ArtModel()
        {

        }
        public ArtModel(Vector2 position, AnimationModel[] animations) : base(position)
        {
            this.animations = animations;
        }
    }
}
