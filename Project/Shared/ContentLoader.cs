using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Model.Gameplay;
using Model.Screen;
using Platman.Component.Base;
using Platman.Component.GameScreen.Screens.Interface;

namespace Platman
{
    public class ContentLoader
    {
        private static ContentLoader _contentLoader;

        public static ContentLoader Instance
        {
            get
            {
                if (_contentLoader == null)
                    _contentLoader = new ContentLoader();
                return _contentLoader;
            }
        }

        private ContentLoader()
        {

        }

        public Texture2D LoadTextureByPath(string texture)
        {
            return GameRoot.Instance.Content.Load<Texture2D>(texture);
        }


        public Song LoadSong(string songName)
        {
            return GameRoot.Instance.Content.Load<Song>("Audio/Song/" + songName);
        }

        public SpriteFont LoadFont(TextSize size)
        {
            return GameRoot.Instance.Content.Load<SpriteFont>("Fonts/bitdust" + size);
        }

        public SoundEffect LoadSound(string name)
        {
            return GameRoot.Instance.Content.Load<SoundEffect>("Audio/Sound/" + name);
        }

        public LevelModel LoadLevel(int stageIndex, int levelIndex)
        {
            return GameRoot.Instance.Content.Load<LevelModel>("Map/Stage" + stageIndex + "/level" + levelIndex);
        }

        public ScreenModel LoadScreen(ScreenContent name)
        {
            return GameRoot.Instance.Content.Load<ScreenModel>("Screen/" + name);
        }


    }
}
