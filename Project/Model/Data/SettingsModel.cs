using Microsoft.Xna.Framework;

namespace Model.Data
{
    public class SettingsModel
    {

        public bool AudioMusicEnabled;
        public int VolumeMusic;

        public bool AudioSoundEnabled;
        public int VolumeSound;

        public Point Resolution; 

        public SettingsModel(bool audioMusicEnabled, int volumeMusic, bool audioSoundEnabled, int volumeSound, Point resolution)
        {   
            AudioMusicEnabled = audioMusicEnabled;
            AudioSoundEnabled = audioSoundEnabled;
            VolumeMusic = volumeMusic;
            VolumeSound = volumeSound;
            Resolution = resolution;
        }

        public SettingsModel()
        {

        }
    }
}
