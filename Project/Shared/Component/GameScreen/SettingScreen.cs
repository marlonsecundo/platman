using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Model.Data;
using Platman.Component.Audio;
using Platman.Component.Base;
using Platman.Component.GameScreen.Screens.Interface;
using Platman.Component.GameScreen.Screens.Interface.Button;
using Platman.Component.GameScreen.Screens.Interface.Menu;
using Platman.Component.Managers;
using Platman.DataBase;
using System.Collections.Generic;
using System.Linq;

namespace Platman.Component.GameScreen.Screens
{
    public sealed partial class SettingScreen : BaseScreen
    {
        private ProgressBar[] progressBar;
        private SoundEffect soundEffect;
        private List<int[]> resolutions;
        private int resolutionIndex;
        Menu menu;

        public SettingScreen() : base(ScreenContent.settings)
        {
            soundEffect = ContentLoader.Instance.LoadSound("shoot");
            menu = (Menu)AllComps.First(t => t is Menu);
        }

        public override void Show(float alpha)
        {
            base.Show(alpha);


            switch (Device.Instance.DeviceType)
            {
                case DType.Android:
                case DType.IOS:
                case DType.Win10_Phone:
                    menu.Itens[2].Enabled = false;
                    break;
            }

            menu.FocusItem(0, false);
        }

        public override void Hide()
        {
            base.Hide();
            menu.DeFocus();
        }

        private void LoadResolutions()
        {
            resolutions = new List<int[]>();
            List<int[]> res = new List<int[]> { new int[] { 1920, 1080 }, new int[] { 1680, 1050 }, new int[] { 1600, 900 }, new int[] { 1440, 900 }, new int[] { 1366, 768 }, new int[] { 1280, 720 }, new int[] { 1024, 768 }, new int[] { 800, 600 } };

            int[] size = new int[] { Settings.Instance.Resolution.X, Settings.Instance.Resolution.Y };
            // Prequiça de explicar se der errado faz de novo, né difícil não. :) {...}
         
            if (!res.Exists(element => element[0] == size[0] && element[1] == size[1]))
            {
                List<int> lsort = new List<int> { 1920, 1680, 1600, 1440, 1366, 1280, 1024, 800 };
                lsort.Add(size[0]);
                int[] sort = lsort.ToArray();
                System.Array.Sort(sort);
                lsort = new List<int>(sort);

                res.Reverse();
                int index = lsort.FindIndex(element => element == size[0]);

                for (int i = 0; i < lsort.Count-1; i++)
                {
                    if (i != index)
                        resolutions.Add(res[i]);
                    else
                    {
                        resolutions.Add(size);
                        index = -1;
                        i--;
                    }
                }

                resolutions.Reverse();

            }
            else
            {
                resolutions = res;
            }

            resolutionIndex = resolutions.FindIndex(element => element[0] == size[0] && element[1] == size[1]);

            Texts[13].Text = size[0] + "X" + size[1];
         
        }

        private void SetResolution()
        {
            int[] size = resolutions[resolutionIndex];
            ResolutionManager.Instance.ResetResolution(new Point(size[0], size[1]));
            CamManager.Instance.RecalculateTransformationMatrices();
            Texts[13].Text = size[0] + "X" + size[1];
        }

        protected override GameButton[] LoadButton()
        {
            GameButton[] buttons = new GameButton[9];

            buttons[0] = new ButtonMark(ButtonAudioMusic_OnClick, this, Model.buttons[1], text : Texts[5]);
            buttons[1] = new GameButton(Model.buttons[2], this, ButtonVolumeMusicLess_OnCLick, text : Texts[3]);
            buttons[2] = new GameButton(Model.buttons[0], this, ButtonVolumeMusicMore_OnCLick, text : Texts[4]);

            buttons[3] = new ButtonMark(ButtonAudioSound_OnClick, this, Model.buttons[4], text : Texts[10]);
            buttons[4] = new GameButton(Model.buttons[3], this, ButtonVolumeSoundMore_OnCLick, text : Texts[7]);
            buttons[5] = new GameButton(Model.buttons[5], this, ButtonVolumeSoundLess_OnCLick, text : Texts[8]);

            buttons[6] = new GameButton(Model.buttons[6], this, ButtonMain_OnClick, text : Texts[11]);

            buttons[7] = new GameButton(Model.buttons[7], this, ButtonLessResolution_OnClick, text : Texts[14]);
            buttons[8] = new GameButton(Model.buttons[8], this, ButtonMoreResolution_OnClick, text : Texts[15]);

       

            return buttons;
        }

        protected override DrawableBase[] LoadOutherComps()
        {
            progressBar = new ProgressBar[2];

            progressBar[0] = new ProgressBar(Settings.Instance.VolumeMusic, Model.comps[0]);
            progressBar[1] = new ProgressBar(Settings.Instance.VolumeSound, Model.comps[1]);

            if (Settings.Instance.AudioMusicEnabled)
            {
                Buttons[0].State = ButtonState.Pressed;
                Texts[2].Text = "ATIVADO";
            }
            else
            {
                Buttons[0].State = ButtonState.Released;
                Texts[2].Text = "DESATIVADO";
            }

            Buttons[0].ChangeAnimation(Buttons[0].State);

            if (Settings.Instance.AudioSoundEnabled)
            {
                Buttons[3].State = ButtonState.Pressed;
                Texts[6].Text = "ATIVADO";

            }
            else
            {
                Buttons[3].State = ButtonState.Released;
                Texts[6].Text = "DESATIVADO";
            }

            Buttons[3].ChangeAnimation(Buttons[3].State);

            LoadResolutions();

            return progressBar;
        }

        protected override Menu LoadMenu()
        {
            MenuItem[] menuItens = new MenuItem[3];

            menuItens[0] = new MenuItem(0);
            menuItens[1] = new MenuItem(1);
            menuItens[2] = new MenuItem(2);

            menuItens[0].AddComp(Buttons[0]);
            menuItens[0].AddComp(Buttons[1]);
            menuItens[0].AddComp(Buttons[2]);
            menuItens[0].AddComp(Texts[1]);
            menuItens[0].AddComp(Texts[2]);
            menuItens[0].AddComp(Comps[0]);

            menuItens[1].AddComp(Buttons[3]);
            menuItens[1].AddComp(Buttons[4]);
            menuItens[1].AddComp(Buttons[5]);
            menuItens[1].AddComp(Texts[6]);
            menuItens[1].AddComp(Texts[9]);
            menuItens[1].AddComp(Comps[1]);

            for (int i = 12; i < 16; i++) menuItens[2].AddComp(Texts[i]);

            menuItens[2].AddComp(Buttons[7]);
            menuItens[2].AddComp(Buttons[8]);

            

            return new Menu(menuItens, this, ItemOrientation.Vertical);
        }

    }

    public sealed partial class SettingScreen : BaseScreen
    {

        private void ButtonLessResolution_OnClick(object sender)
        {
            resolutionIndex++;

            if (resolutionIndex >= resolutions.Count) resolutionIndex = 0;

            SetResolution();
        }

        private void ButtonMoreResolution_OnClick(object sender)
        {
            resolutionIndex--;

            if (resolutionIndex < 0) resolutionIndex = resolutions.Count - 1;

            SetResolution();

        }


        public void ButtonAudioMusic_OnClick(object sender)
        {
            switch ((sender as GameButton).State)
            {
                case ButtonState.Pressed:
                    Texts[2].Text = "ATIVADO";
                    Settings.Instance.AudioMusicEnabled = true;
                    break;
                case ButtonState.Released:
                    Texts[2].Text = "DESATIVADO";
                    Settings.Instance.AudioMusicEnabled = false;
                    break;
            }

            if (Settings.Instance.AudioMusicEnabled)
                GameMusicPlayer.Instance.PlayScreenMusic();
            else
                GameMusicPlayer.Instance.StopMusic();

            GameMusicPlayer.Instance.PlayScreenMusic();
        }

        public void ButtonAudioSound_OnClick(object sender)
        {
            switch ((sender as GameButton).State)
            {
                case ButtonState.Pressed:
                    Texts[6].Text = "ATIVADO";
                    Settings.Instance.AudioSoundEnabled = true;
                    break;
                case ButtonState.Released:
                    Texts[6].Text = "DESATIVADO";
                    Settings.Instance.AudioSoundEnabled = false;
                    break;
            }
        }

        public void ButtonVolumeMusicMore_OnCLick(object sender)
        {
            progressBar[0].Value++;
            Settings.Instance.VolumeMusic = progressBar[0].Value;
        }
        public void ButtonVolumeMusicLess_OnCLick(object sender)
        {
            progressBar[0].Value--;
            Settings.Instance.VolumeMusic = progressBar[0].Value;
        }

        public void ButtonVolumeSoundMore_OnCLick(object sender)
        {
            progressBar[1].Value++;
            Settings.Instance.VolumeSound = progressBar[1].Value;
            GameSoundPlayer.Instance.PlaySound(soundEffect);
        }
        public void ButtonVolumeSoundLess_OnCLick(object sender)
        {
            progressBar[1].Value--;
            Settings.Instance.VolumeSound = progressBar[1].Value;
            GameSoundPlayer.Instance.PlaySound(soundEffect);
        }

        private void ButtonMain_OnClick(object sender)
        {
            var inst = Settings.Instance;
            DataManager.Instance.Save(Filename.Setting, new SettingsModel(inst.AudioMusicEnabled, inst.VolumeMusic, inst.AudioSoundEnabled, inst.VolumeSound, inst.Resolution));
            ScreenManager.Instance.SwitchScreen(ScreenContent.main);
        }


    }
}
