using Microsoft.Xna.Framework.Audio;

namespace Platman.Component.Audio
{
    public partial class GameSoundPlayer
    {
        private SoundEffectInstance soundEffect;
        public bool AudioEnabled { get; set; }
        public int VolumeAudio { get; set; }

        private GameSoundPlayer()
        {

        }

        public void PlaySound(SoundEffect sound)
        {
            if (AudioEnabled)
            {
                soundEffect = sound.CreateInstance();
                soundEffect.Volume = VolumeAudio / 10f;
                soundEffect.Play();
            }
        }


    }

    public partial class GameSoundPlayer
    {
        private static GameSoundPlayer _instance;

        public static GameSoundPlayer Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameSoundPlayer();

                return _instance;
            }
        }
    }
}
