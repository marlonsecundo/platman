using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Platman.Component.Audio;
using Platman.Component.Base;
using Platman.Component.Effect;
using Platman.Component.Gameplay;
using Platman.Component.GameScreen.Screens;
using Platman.Component.Input;
using System;
using System.Collections.Generic;

namespace Platman.Component.Managers
{
    public partial class ScreenManager : GameComponent
    {
        public event TransitionEventHandler TransitionComplete;

        private SoundEffect soundEffect;
        private FadeEffect fadeEffect;
        private BaseScreen[] activeScreens;
        private BaseScreen setScreen;

        Dictionary<ScreenContent, BaseScreen> screenDict;
        private ScreenManager() : base(GameRoot.Instance)
        {
            TransitionComplete += ScreenManager_TransitionComplete;
            Enabled = false;
            activeScreens = new BaseScreen[3];
            
            screenDict = new Dictionary<ScreenContent, BaseScreen>
            {
                { ScreenContent.main, new MainScreen() },
                { ScreenContent.settings, new SettingScreen() },
                { ScreenContent.stage, new StageScreen() },
                { ScreenContent.help, new HelpScreen() },
                { ScreenContent.about, new AboutScreen() }
            };

            soundEffect = ContentLoader.Instance.LoadSound("screen");

            activeScreens[0] = screenDict[ScreenContent.main];
            activeScreens[0].Show(1f);

            Game.Components.Add(this);
        }


        private void ScreenManager_TransitionComplete()
        {
            if (activeScreens[0].GetType() == typeof(MainScreen))
            {
                if (GameMusicPlayer.Instance.CurrentType != GameSongType.Screen)
                    GameMusicPlayer.Instance.PlayScreenMusic();
            }
        }

        public void SwitchScreen(ScreenContent content)
        {
            GameSoundPlayer.Instance.PlaySound(soundEffect);

            PlayerInput.Instance.Enabled = false;
            setScreen = screenDict[content];

            fadeEffect = new FadeEffect(true, 25, 1f, 0f);
            fadeEffect.IsFocus = false;

            fadeEffect.Finish += FadeEffect_Completed;
            Enabled = true;
        }

        public void ShowInputScreen(InputScreen instance)
        {
            activeScreens[2] = instance;
            activeScreens[2].Show(1f);
        }

        public void SwitchLevelScreen(int index)
        {
            GameSoundPlayer.Instance.PlaySound(soundEffect);

            PlayerInput.Instance.Enabled = false;
            setScreen = new LevelScreen(index);

            fadeEffect = new FadeEffect(true, 25, 1f, 0f);
            fadeEffect.IsFocus = false;

            fadeEffect.Finish += FadeEffect_Completed;
            Enabled = true;
        }

        public void SwitchScreen(BaseScreen content)
        {
            var instance = ScreenTransitionManager.NewInstance;
            instance.StartTransition(content, activeScreens[0], ShowScreen);
            instance.Finished += ScreenManager_FinishedTrasitEvent;
        }


        private void ShowScreen(BaseScreen screen, float alpha)
        {
            activeScreens[0].Unload();
            activeScreens[0] = null;
            activeScreens[0] = screen;
            activeScreens[0].Show(alpha);
        }
        public void ShowGameScreen(int stage, int level)
        {
            GameLevelScreen screen = new GameLevelScreen(stage, level);

            var instance = ScreenTransitionManager.NewInstance;
            instance.StartTransition(screen, activeScreens[0], ShowScreen);
            instance.Finished += ScreenManager_FinishedTrasitEvent;
        }

        private void SwapScreens(BaseScreen screen, float alpha)
        {
            activeScreens[0].Hide();
            activeScreens[0] = null;
            activeScreens[0] = screen;
            activeScreens[0].Show(alpha);
        }

        private void ScreenManager_FinishedTrasitEvent()
        {
            TransitionComplete();
        }
    }



    public partial class ScreenManager : GameComponent
    {
        private static ScreenManager _instance;
        public static ScreenManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ScreenManager();

                return _instance;
            }
        }


        int time;
        bool activate;
        public override void Update(GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime.Milliseconds;

            if (time > 150)
            {
                time = 0;
                activate = true;
            }

            if (activate)
            { 
                fadeEffect.RunEffect(gameTime);
                activeScreens[0].Alpha = fadeEffect.Alpha;
            }
        }

        private void FadeEffect_Completed(object sender, EventArgs e)
        {
            if (!fadeEffect.IsFocus)
            {
                SwapScreens(setScreen, 0);
                fadeEffect.IsFocus = true;
            }
            else if (fadeEffect.IsFocus)
            {
                activate = false;
                Enabled = false;
                PlayerInput.Instance.Enabled = true;
                TransitionComplete();
            }
        }
    }
}
