using Microsoft.Xna.Framework.Media;
using System;

namespace Platman.Component.Audio
{
    public enum GameSongType
    {
        Screen,
        Game
    }
    public class GameMusicPlayer
    {
        private float _volumeMusic;
        private bool _soundEnabled;
        public int VolumeMusic { get => Convert.ToInt32(_volumeMusic * 10); set => Microsoft.Xna.Framework.Media.MediaPlayer.Volume = _volumeMusic = value / 10f; }    
       
        public bool SoundEnabled { get => _soundEnabled; set => _soundEnabled = value; }

        public GameSongType CurrentType { get; private set; }
        private string CurrentGameMusic { get; set; }

        private GameMusicPlayer()
        {
            Microsoft.Xna.Framework.Media.MediaPlayer.IsRepeating = true;
        }

        public void PlayScreenMusic()
        {
            var musics = new string[] { "Bit Adventure", "Bit Select" };

            Random rnd = new Random();
            int index = rnd.Next(0, musics.Length);

            Song music = ContentLoader.Instance.LoadSong(GameSongType.Screen + "/" + musics[index]);

            Play(music, GameSongType.Screen);
        }

        public void PlayGameMusic(Song music)
        {
            if (music.Name != CurrentGameMusic || Microsoft.Xna.Framework.Media.MediaPlayer.State != MediaState.Playing)
            {
                Play(music, GameSongType.Game);
                CurrentGameMusic = music.Name;
            }
        }

        private void Play(Song music, GameSongType type)
        {
            if (SoundEnabled)
            {
                CurrentType = type;

                Microsoft.Xna.Framework.Media.MediaPlayer.Volume = _volumeMusic;

                Microsoft.Xna.Framework.Media.MediaPlayer.Play(music);
            }
        }

        public void StopMusic()
        {
            Microsoft.Xna.Framework.Media.MediaPlayer.Stop();
        }

        private static GameMusicPlayer _instance;

        public static GameMusicPlayer Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameMusicPlayer();

                return _instance;
            }
        }

    }
}
