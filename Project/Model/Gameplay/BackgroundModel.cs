using Microsoft.Xna.Framework;
using Model.Screen;
using System.Collections.Generic;

namespace Model.Gameplay
{
    public class BackgroundModel
    {
        public Vector2 cameraPosition;
        public ArtModel background;
        public List<ArtModel> arts;
        public BackgroundModel(Vector2 cameraPosition, ArtModel background, List<ArtModel> arts)
        {
            this.cameraPosition = cameraPosition;
            this.background = background;
            this.arts = arts;
        }

        public BackgroundModel()
        {

        }
    }
}
