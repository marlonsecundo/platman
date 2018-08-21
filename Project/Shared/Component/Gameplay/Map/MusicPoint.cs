using Microsoft.Xna.Framework.Media;
using Model.Gameplay;
using Platman.Component.Audio;
using VelcroPhysics.Dynamics;

namespace Platman.Component.Gameplay.Map
{
    public class MusicPoint : Block
    {
        private Song Music { get; }
        public MusicPoint(MusicPointModel model, World world) : base(model, world)
        {
            Music = ContentLoader.Instance.LoadSong(GameSongType.Game + "/" + model.music);
        }

        public void Play()
        {
            GameMusicPlayer.Instance.PlayGameMusic(Music);
        }
    }
}
