using Microsoft.Xna.Framework;
using Model.Base;
using System.Collections.Generic;

namespace Model.Gameplay
{
    public class LevelModel
    {
        public Rectangle bounds;
        public BackgroundModel background;
        public List<BlockModel> blocks;
        public List<BlockModel> deathPoints;
        public List<TextPointModel> textPoints;
        public List<MusicPointModel> musicPoints;
        public List<PoetryModel> poetryPoints;
        public List<GravityModel> gravityPoints;
        public List<BlockModel> finalPoints;
        public CompModel hero;
        public CompModel camera;

        public LevelModel()
        {

        }

        public LevelModel(Rectangle bounds, List<BlockModel> blocks, List<BlockModel> deathPoints, List<TextPointModel> textPoints, List<MusicPointModel> musicPoints, List<PoetryModel> poetryPoints, List<GravityModel> gravityPoints, List<BlockModel> finalPoints, BackgroundModel background, CompModel camera, CompModel hero)
        {
            this.bounds = bounds;
            this.blocks = blocks;
            this.deathPoints = deathPoints;
            this.textPoints = textPoints;
            this.musicPoints = musicPoints;
            this.poetryPoints = poetryPoints;
            this.gravityPoints = gravityPoints;
            this.finalPoints = finalPoints;
            this.background = background;
            this.hero = hero;
            this.camera = camera;
        }
    }
}
