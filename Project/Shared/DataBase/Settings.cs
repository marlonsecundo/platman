using Microsoft.Xna.Framework;
using Model.Data;
using Platman.Component.Audio;
using Platman.Component.Managers;

namespace Platman.DataBase
{
    public partial class Settings
    {
        public SettingsModel Model { get; }

        public Settings(SettingsModel model)
        {
            Model = model;
            VolumeMusic = model.VolumeMusic;
            AudioMusicEnabled = model.AudioMusicEnabled;
            VolumeSound = model.VolumeSound;
            AudioSoundEnabled = model.AudioSoundEnabled;
        }

        public bool AudioMusicEnabled
        {
            get { return GameMusicPlayer.Instance.SoundEnabled; }
            set { GameMusicPlayer.Instance.SoundEnabled = value; }
        }

        public int VolumeMusic
        {
            get { return GameMusicPlayer.Instance.VolumeMusic; }
            set { GameMusicPlayer.Instance.VolumeMusic = value; }
        }

        public bool AudioSoundEnabled
        {
            get { return GameSoundPlayer.Instance.AudioEnabled; }
            set { GameSoundPlayer.Instance.AudioEnabled = value; }
        }

        public int VolumeSound
        {
            get { return GameSoundPlayer.Instance.VolumeAudio; }
            set { GameSoundPlayer.Instance.VolumeAudio = value; }
        }

        public Point Resolution
        {
            get => ResolutionManager.Instance.ScreenSize;
            set
            {
                ResolutionManager.Instance.ResetResolution(value);
                CamManager.Instance.RecalculateTransformationMatrices();
            }
        }



    }


    public partial class Settings
    {
        private static Settings _instance;

        public static Settings Instance
        {
            get
            {
                if (_instance == null)
                {
                    SettingsModel model = DataManager.Instance.Load<SettingsModel>(Filename.Setting);

                    _instance = new Settings(model);
                }
                return _instance;
            }
        }
    }
}
